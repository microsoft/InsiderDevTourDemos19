var residentalbutton, commercialbutton, otherbutton, bronzebutton, silverbutton, platinumbutton, chatbutton, viewInvoice;

window.addEventListener("load", createBingMap);
document.addEventListener("DOMContentLoaded", readyToGo);

function createBingMap() {
    var map = new Microsoft.Maps.Map('#myMap', {
        credentials: 'AkGhzkFjIEThiHDN70dByXzB2C7AZL686tWK02yaDYYgcZckFEunOB7qRUk-l8HQ',
        center: new Microsoft.Maps.Location(47.684059, -122.252565),
        mapTypeId: Microsoft.Maps.MapTypeId.road,
        navigationBarMode: Microsoft.Maps.NavigationBarMode.compact,
        zoom: 15
    });

    var center = map.getCenter();

    //Create custom Pushpin
    var pin = new Microsoft.Maps.Pushpin(center, {
        title: 'Toby Fitch',
        subTitle: 'Customer',
        color: Microsoft.Maps.Color.fromHex('#c3c3c3c')
    });

    //Add the pushpin to the map
    map.entities.push(pin);
}

function readyToGo() {
    residentalbutton = document.getElementsByClassName("ResidentalType")[0];
    residentalbutton.addEventListener("click", buttonPropertyTypeClicked);
    commercialbutton = document.getElementsByClassName("CommercialType")[0];
    commercialbutton.addEventListener("click", buttonPropertyTypeClicked);
    otherbutton = document.getElementsByClassName("OtherType")[0];
    otherbutton.addEventListener("click", buttonPropertyTypeClicked);
    bronzebutton = document.getElementsByClassName("BronzeType")[0];
    bronzebutton.addEventListener("click", buttonPackageTypeClicked);
    silverbutton = document.getElementsByClassName("SilverType")[0];
    silverbutton.addEventListener("click", buttonPackageTypeClicked);
    platinumbutton = document.getElementsByClassName("PlatinumType")[0];
    platinumbutton.addEventListener("click", buttonPackageTypeClicked);
    chatbutton = document.getElementsByClassName("fa-comment-dots")[0];
    chatbutton.addEventListener("click", chatbuttonClicked);
    viewInvoice = document.getElementById("viewInvoice");
    viewInvoice.addEventListener("click", viewInvoiceClicked);
}

function chatbuttonClicked(event) {
    window.open("./chat/index.html", "_blank");
}

function viewInvoiceClicked(event) {
    window.open("./invoice.html", "_blank");
}

function buttonPropertyTypeClicked(event) {
    residentalbutton.classList.remove("buttonSelected");
    commercialbutton.classList.remove("buttonSelected");
    otherbutton.classList.remove("buttonSelected");
    if (event.target.nodeName !== "BUTTON") {
        event.target.parentElement.classList.add("buttonSelected");
    }
    else {
        event.target.classList.add("buttonSelected");
    }
}

function buttonPackageTypeClicked(event) {
    bronzebutton.classList.remove("buttonSelected");
    silverbutton.classList.remove("buttonSelected");
    platinumbutton.classList.remove("buttonSelected");
    if (event.target.nodeName !== "BUTTON") {
        event.target.parentElement.classList.add("buttonSelected");
    }
    else {
        event.target.classList.add("buttonSelected");
    }
}


navigator.serviceWorker.ready.then(function (registration) {
    // Check if the user has an existing subscription
    return registration.pushManager.getSubscription()
        .then(async function (subscription) {
            return subscription;
    });
}).then(function (subscription) {
    document.getElementById('contactCustomer').addEventListener('click', function () {
        fetch('./sendNotification', {
            method: 'post',
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify({
                subscription: subscription
            })
        });
    });
});
