'use strict';
var express = require('express');
var router = express.Router();
var webpush = require('web-push');
var Datastore = require('nedb');
var path = require('path');

require('dotenv').config();

/* GET home page. */
router.get('/', function (req, res) {
    res.render('index', { title: 'PWA Web Push Notifications' });
});

router.get('/offline.html', function (req, res) {
  res.sendFile('public/offline.html');
});

webpush.setVapidDetails(
  'mailto:' + process.env.VAPID_EMAIL,
  process.env.VAPID_PUBLICKEY,
  process.env.VAPID_PRIVATEKEY
);

var db = new Datastore({
  filename: path.join(__dirname, 'subscriptions.db'),
  autoload: true
});

function saveSubscriptionToDatabase(subscription) {
  return new Promise(function (resolve, reject) {
    db.update({ endpoint: subscription.endpoint }, subscription, { upsert: true }, function (err, newDoc) {
      if (err) {
        reject(err);
        return;
      }
      resolve(newDoc._id);
    });
  });
};

function getSubscriptionsFromDatabase() {
  return new Promise(function (resolve, reject) {
    db.find({}, function (err, docs) {
      if (err) {
        reject(err);
        return;
      }
      resolve(docs);
    });
  });
}

function deleteSubscriptionFromDatabase(subscriptionId) {
  return new Promise(function (resolve, reject) {
    db.remove({ _id: subscriptionId }, {}, function (err) {
      if (err) {
        reject(err);
        return;
      }
      resolve();
    });
  });
}

const isValidSaveRequest = (req, res) => {
  // Check the request body has at least an endpoint.
  if (!req.body || !req.body.subscription || !req.body.subscription.endpoint) {
    // Not a valid subscription.
    res.status(400);
    res.setHeader('Content-Type', 'application/json');
    res.send(JSON.stringify({
      error: {
        id: 'invalid-subscription',
        message: 'received invalid subscription'
      }
    }));
    return false;
  }
  return true;
};


router.get('/vapidPublicKey', function (req, res) {
  res.send(process.env.VAPID_PUBLICKEY);
});

router.post('/register', function (req, res) {
  if (!isValidSaveRequest(req, res)) {
    return;
  }

  return saveSubscriptionToDatabase(req.body.subscription)
    .then(function (subscriptionId) {
      res.setHeader('Content-Type', 'application/json');
      res.send(JSON.stringify({ data: { success: true } }));
    })
    .catch(function (err) {
      res.status(500);
      res.setHeader('Content-Type', 'application/json');
      res.send(JSON.stringify({
        error: {
          id: 'unable-to-save-subscription',
          message: 'The subscription was received but we were unable to save it to our database.'
        }
      }));
    });
});

var notification = {
  "notification": {
    "title": "Northwind App",
    "body": "Customer chat is ready.",
    "icon": "./chat/images/f78fda56-b66c-c7ab-a23c-4550e3ae5e76.webPlatform.png"
  }
};

const triggerPushMsg = function (subscription, dataToSend) {
  return webpush.sendNotification(subscription, dataToSend)
    .catch((err) => {
      if (err.statusCode === 410) {
        return deleteSubscriptionFromDatabase(subscription._id);
      } else {
        console.log('Subscription is no longer valid: ', err);
      }
    });
};

router.post('/sendNotification', function (req, res) {
  // NOTE: This API endpoint should be secure (i.e. protected with a login
  // check OR not publicly available.)

  const dataToSend = JSON.stringify(notification);

  return getSubscriptionsFromDatabase()
    .then(function (subscriptions) {
      let promiseChain = Promise.resolve();

      for (let i = 0; i < subscriptions.length; i++) {
        const subscription = subscriptions[i];
        promiseChain = promiseChain.then(() => {
          return triggerPushMsg(subscription, dataToSend);
        });
      }

      return promiseChain;
    })
    .then(() => {
      res.setHeader('Content-Type', 'application/json');
      res.send(JSON.stringify({ data: { success: true } }));
    })
    .catch(function (err) {
      res.status(500);
      res.setHeader('Content-Type', 'application/json');
      res.send(JSON.stringify({
        error: {
          id: 'unable-to-send-messages',
          message: `We were unable to send messages to all subscriptions : ` +
            `'${err.message}'`
        }
      }));
    });
});

module.exports = router;
