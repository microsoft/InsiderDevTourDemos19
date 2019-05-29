var webpush = require('web-push');
//console.log(webpush.generateVAPIDKeys());

var vapidPublicKey = 'BEE59Vgp6HoLCg2YDbyPAL9-ZKqXLM6bsH6a4psZVa_mOt4-6HsOjjEINvAMQhNduwxewr0YkozlREtZpMrG7dQ';
var vapidPrivateKey = 'ey3fB6s8X-9r4F3SaN8ZiRx54aOKk2m-sZqEEVj2hVc';
        
webpush.setVapidDetails(
    'mailto:david_rousset@hotmail.com',
    vapidPublicKey,
    vapidPrivateKey
);

var express = require('express');
var app = express();

app.get('/api/key', function(req, res) {
    res.send({
        key: vapidPublicKey
    });
});

app.post('/api/save-subscription', function(req, res) {
    // save req.body.subscription to a database

    res.send('Success');
});

webPush.sendNotification(savedSubscriptionData, payload)
    .then(function (response) {
        console.log('sent');
    });