// Create a Stripe client.
var stripe = Stripe('pk_test_9VBmb0MLFOio86PO162GMNFp');

// Create an instance of Elements.
var elements = stripe.elements();



// Custom styling can be passed to options when creating an Element.
// (Note that this demo uses a wider set of styles than the guide below.)
var style = {
    base: {
        color: '#32325d',
        lineHeight: '18px',
        fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
        fontSmoothing: 'antialiased',
        fontSize: '16px',
        '::placeholder': {
            color: '#d0d0d0'
        }
    },
    invalid: {
        color: '#fa755a',
        iconColor: '#fa755a'
    }
};

//Stripe payment button
var paymentRequest = stripe.paymentRequest({
    country: 'US',
    currency: 'usd',
    total: {
        label: 'Demo total',
        amount: 1999
    }
});

var elements = stripe.elements();
var prButton = elements.create('paymentRequestButton', {
    paymentRequest: paymentRequest
});

// Check the availability of the Payment Request API first.
paymentRequest.canMakePayment().then(function (result) {
    //console.log("in canMakePayment");
    if (result) {
        prButton.mount('#StripePaymentRequestButton');
    } else {
        document.getElementById('StripePaymentRequestButton').style.display = 'disabled';
    }
});

paymentRequest.on('token', function (ev) {
    // Send the token to your server to charge it!
    fetch('/charges', {
        method: 'POST',
        body: JSON.stringify({ token: ev.token.id }),
        headers: { 'content-type': 'application/json' }
    })
        .then(function (response) {
            if (response.ok) {
                // Report to the browser that the payment was successful, prompting
                // it to close the browser payment interface.
                alert("Payment Successful!");
                ev.complete('success');
            } else {
                // Report to the browser that the payment failed, prompting it to
                // re-show the payment interface, or show an error message and close
                // the payment interface.
                alert("Payment Failed!");
                ev.complete('fail');
            }
        });
});


// Create an instance of the card Element.
var card = elements.create('card', { style: style });

// Add an instance of the card Element into the `card-element` <div>.
card.mount('#card-element');

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
var form = document.getElementById('StripeForm');
form.addEventListener('submit', function (event) {
    event.preventDefault();
    $("#subBtn").addClass("disabled");
    stripe.createToken(card).then(function (result) {
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
    var form = document.getElementById('StripeForm');
    // tokenId
    var hiddenInput = document.createElement('input');
    hiddenInput.setAttribute('type', 'hidden');
    hiddenInput.setAttribute('name', 'StripeData.tokenId');
    hiddenInput.setAttribute('value', token.id);
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
    // Submit the form
    form.submit();
}

// Checkout Custom Payment
var custHandler = StripeCheckout.configure({
    key: 'pk_test_9VBmb0MLFOio86PO162GMNFp',
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

document.getElementById('subCustPayBtn').addEventListener('click', function (e) {
    // Open Checkout with further options:
    custHandler.open({
        name: 'Challenger Technology Solutions',
        description: '2 widgets',
        amount: 2345,
        zipCode: true,
        email: "test@darryl.com",
        billingaddress: true
    });
    e.preventDefault();
});

// Close Checkout on page navigation:
window.addEventListener('popstate', function () {
    handler.close();
});

