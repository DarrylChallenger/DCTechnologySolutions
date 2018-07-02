// Create a Stripe client.
var stripe = Stripe($('#publicKey').val());

// Create an instance of Elements.
var elements = stripe.elements();

// This style is for the PayNow btn 
var style = {
    base: {
        color: 'black',//#32325d',
        lineHeight: '18px',
        fontFamily: '"Russo One", Helvetica, sans-serif',
        fontSmoothing: 'antialiased',
        fontSize: '16px',
        '::placeholder': {
            //color: 'midnightblue'//#d0d0d0'
        }
    },
    invalid: {
        color: '#fa755a',
        iconColor: '#fa755a'
    }
};

//Stripe pay now button
var paymentRequest = stripe.paymentRequest({
    country: 'US',
    currency: 'usd',
    total: {
        label: 'Demo total',
        amount: 1999
    },
    requestShipping: true,
    shippingOptions: [
        {
            id: 'free-shipping',
            label: 'Free shipping',
            detail: 'Arrives in 5 to 9 days',
            amount: 0,
        },
    ],
});

var elements = stripe.elements();
var prButton = elements.create('paymentRequestButton', {
    paymentRequest: paymentRequest
});

// Check the availability of the Payment Request API first.
paymentRequest.canMakePayment().then(function (result) {
    console.log("in canMakePayment");
    if (result) {
        console.log("in canMakePayment mounting");
        prButton.mount('#StripeSamplesPayNowBtn');
    } else {
        console.log("in canMakePayment disabled");
        //$('#StripeSamplesPayNowBtn').style.display = 'disabled';
        $('#StripeSamplesPayNowBtn').html("<h5>No Payment methods detected on this session - please add a card to Apple Pay, Google Pay, or Microsoft Pay.</h5>");
    }
});

paymentRequest.on('token', function (ev) {
    // Send the token to your server to charge it!
    console.log("In paymentRequest.on-");
    console.log(ev);
    console.log(ev.methodName);
    console.log(ev.shippingAddress);
    fetch('/Gallery/ProcessPayNow', {
        method: 'POST',
        body: JSON.stringify( {
            token: ev.token.id,
            stripeKey : $('#stripeKey').val(),
            email : $('#mailAddr').val(),
            orderId: 'C4711B90-57E7-4585-84D2-AA269A275DC4',
            shippingAddress: ev.shippingAddress
         }),
        headers: { 'content-type': 'application/json' }
    })
        .then(function (response) {
            console.log("Response-");
            console.log(response);
            if (response.ok) {
                console.log("Ok");
                // Report to the browser that the payment was successful, prompting
                // it to close the browser payment interface.
                ev.complete('success');
            } else {
                console.log("response.ok is false");
                // Report to the browser that the payment failed, prompting it to
                // re-show the payment interface, or show an error message and close
                // the payment interface.
                ev.complete('fail');
            }
            console.log('b4 return');
            return response.json();
        }).then(function (jsdata) {
            console.log('jsdata-');
            console.log(jsdata);
            console.log("nav to /Gallery/DisplayStripeResults");
            // Change to POST
            var s = '/Gallery/DisplayStripeResults?' +

                ((jsdata.charge !== null) ? '&charge.Id=' + jsdata.charge.Id : '') +
                ((jsdata.customer !== null) ? '&customer.Id=' + jsdata.customer.Id : '') +
                ((jsdata.error !== null) ?
                    '&isError=' + jsdata.isError +
                    '&error.ChargeId=' + jsdata.error.ChargeId +
                    '&error.Code=' + jsdata.error.Code +
                    '&error.DeclineCode=' + jsdata.error.DeclineCode +
                    '&error.Error=' + jsdata.error.Error +
                    '&error.ErrorDescription=' + jsdata.error.ErrorDescription +
                    '&error.ErrorType=' + jsdata.error.ErrorType +
                    '&error.Message=' + jsdata.error.Message +
                    '&error.Parameter=' + jsdata.error.Parameter
                        : '');
                //'&error.=' + jsdata.error. +
            console.log(s);
            location.href = s;
        });
});

// This style is for the card 
var cardStyle = {
    base: {
        color: 'midnightblue',//#32325d',
        lineHeight: '18px',
        fontFamily: '"Russo One", Helvetica, sans-serif',
        fontSmoothing: 'antialiased',
        fontSize: '14px',

        '::placeholder': {
            color: 'lightsteelblue',//#d0d0d0',
        }
    },
    invalid: {
        color: '#fa755a',
        //iconColor: 'lightsteelblue'//'#fa755a'
    }
};


// Create an instance of the ProcessCardElement Element.
var card = elements.create('card', { hidePostalCode: true, style: cardStyle });

// Add an instance of the card Element into the `StripeSamplesPayNowBtn` <div>.
card.mount('#StripeCCElement');

// Handle real-time validation errors from the card Element.
card.addEventListener('change', function (event) {
    var displayError = document.getElementById('card-errors');
    if (event.error) {
        displayError.textContent = event.error.message;
    } else {
        displayError.textContent = '';
    }
});

// Handle form submission.
var form = document.getElementById('StripeCCForm');
form.addEventListener('submit', function (event) {
    event.preventDefault();
    $("#subBtn").addClass("disabled");
    // validate required fields
    var additionalData = {
        name: $('#billingAddress_name') ? $('#billingAddress_name').val() : undefined,
        address_line1: $('#billingAddress_address') ? $('#billingAddress_address').val() : undefined,
        address_city: $('#billingAddress_city') ? $('#billingAddress_city').val() : undefined,
        address_state: $('#billingAddress_state') ? $('#billingAddress_state').val() : undefined,
        address_zip: $('#billingAddress_zip') ? $('#billingAddress_postalCode').val() : undefined,
    };
    console.log('additionalData');
    console.log(additionalData);
    stripe.createToken(card, additionalData).then(function (result) { // Add billing address to card
        if (result.error) {
            // Inform the user if there was an error.
            var errorElement = document.getElementById('card-errors');
            errorElement.textContent = result.error.message;
            $("#subBtn").removeClass("disabled");
        } else {
            // Send the token to your server.
            stripeTokenHandler(result.token);
        }
    });
});

function stripeTokenHandler(token) {
    // Insert the token ID into the form so it gets submitted to the server
    var form = document.getElementById('StripeCCForm');
    // tokenId
    var hiddenInput = document.createElement('input');
    hiddenInput.setAttribute('type', 'hidden');
    hiddenInput.setAttribute('name', 'tokenId');
    hiddenInput.setAttribute('value', token.id);
    form.appendChild(hiddenInput);
    // livemode
    hiddenInput = document.createElement('input');
    hiddenInput.setAttribute('type', 'hidden');
    hiddenInput.setAttribute('name', 'livemode');
    hiddenInput.setAttribute('value', token.livemode);
    form.appendChild(hiddenInput);
    // type
    hiddenInput = document.createElement('input');
    hiddenInput.setAttribute('type', 'hidden');
    hiddenInput.setAttribute('name', 'type');
    hiddenInput.setAttribute('value', token.type);
    form.appendChild(hiddenInput);
    // used
    hiddenInput = document.createElement('input');
    hiddenInput.setAttribute('type', 'hidden');
    hiddenInput.setAttribute('name', 'used');
    hiddenInput.setAttribute('value', token.used);
    form.appendChild(hiddenInput);
    console.log(token);
    // Submit the form
    form.submit();
}

// Checkout Custom Payment
var custHandler = StripeCheckout.configure({
    key: $('#publicKey').val(),
    image: 'https://stripe.com/img/documentation/checkout/marketplace.png',
    locale: 'auto',
    token: function (token) {
        // You can access the token ID with `token.id`.
        // Get the token ID to your server-side code for use.
        // Insert the token ID into the form so it gets submitted to the server
        var form = document.getElementById('StripeForm');
        // tokenId
        var hiddenInput = document.createElement('input');
        hiddenInput.setAttribute('type', 'hidden');
        hiddenInput.setAttribute('name', 'StripeData.tokenId');
        hiddenInput.setAttribute('value', token.id);
        form.appendChild(hiddenInput);
        // email
        hiddenInput = document.createElement('input');
        hiddenInput.setAttribute('type', 'hidden');
        hiddenInput.setAttribute('name', 'StripeData.stripeEmail');
        hiddenInput.setAttribute('value', token.email);
        form.appendChild(hiddenInput);
        // livemode
        hiddenInput = document.createElement('input');
        hiddenInput.setAttribute('type', 'hidden');
        hiddenInput.setAttribute('name', 'StripeData.livemode');
        hiddenInput.setAttribute('value', token.livemode);
        form.appendChild(hiddenInput);
        // type
        hiddenInput = document.createElement('input');
        hiddenInput.setAttribute('type', 'hidden');
        hiddenInput.setAttribute('name', 'StripeData.type');
        hiddenInput.setAttribute('value', token.type);
        form.appendChild(hiddenInput);
        // used
        hiddenInput = document.createElement('input');
        hiddenInput.setAttribute('type', 'hidden');
        hiddenInput.setAttribute('name', 'StripeData.used');
        hiddenInput.setAttribute('value', token.used);
        form.appendChild(hiddenInput);
        //console.log(token);
        form.submit();

    }
});
/*
document.getElementById('subCustPayBtn').addEventListener('click', function (e) {
    // Open Checkout with further options:
    console.log("addEventListener");
    custHandler.open({
        name: 'Challenger Technology Solutions',
        description: '2 widgets',
        amount: 2345,
        zipCode: true,
        email: "test@darryl.com",
        billingAddress: true
    });
    e.preventDefault();
});
*/
// Close Checkout on page navigation:
window.addEventListener('popstate', function () {
    handler.close();
});

// Save Card Info 
