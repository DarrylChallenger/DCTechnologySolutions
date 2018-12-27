
function GetCurrentClientId() {
    //var cId = $("#UserClientId").val();
    var cId = $("#ClientId").val();
    return cId;
}

function GetCurrentClientSecretKey() {
    return "SecrectKey";
}

var myClientId = GetCurrentClientId();

paypal.Button.render({
    env: 'sandbox', // Or 'sandbox',
    commit: true, // Show a 'Pay Now' button
    client: {
        sandbox: myClientId
    },
    style: {
        label: 'buynow',
        color: 'gold',
        size: 'responsive',
        shape: 'pill',
        tagline: false,
        fundingicons: false
    },
    payment: function (data, actions) {
        return actions.payment.create({
            payment: {
                transactions: [
                    {
                        amount: { total: '1.01', currency: 'USD' }
                    }
                ]
            }
        });
    },
    onAuthorize: function (data, actions) {
        return actions.payment.execute().then(function (payment) {
            location.href = "PayPalCreatePaymentReturn" +
                '?intent=' + data.intent +
                '&orderId=' + data.orderID +
                '&paymentID=' + data.paymentID +
                '&payerID=' + data.payerID +
                '&returnUrl=' + data.returnUrl;
        });
    },
    onCancel: function (data, actions) {
        /*
            * Buyer cancelled the payment
            */
        location.href = "PayPalCreatePaymentCancel" +
            '?intent=' + data.intent +
            '&paymentID=' + data.paymentID +
            '&payerID=' + data.payerID +
            '&billingId=' + data.billingId +
            '&cancelUrl=' + data.cancelUrl +
            '&token=' + data.paymentToken;

    },
    onError: function (err) {
        /*
            * An error occurred during the transaction
            */
    }
}, '#SimplePayPalBtn');

paypal.Button.render({

        // Set your environment
        env: 'sandbox', // sandbox | production

        // Specify the style of the button

        style: {
            layout: 'vertical', // horizontal | vertical
            size: 'responsive', // medium | large | responsive
            shape: 'pill', // pill | rect
            color: 'gold' // gold | blue | silver | black
        },

// Specify allowed and disallowed funding sources
//
// Options:
// - paypal.FUNDING.CARD
// - paypal.FUNDING.CREDIT
// - paypal.FUNDING.ELV

        funding: {
            allowed: [paypal.FUNDING.CARD, paypal.FUNDING.CREDIT],
            disallowed: []
        },

// PayPal Client IDs - replace with your own
// Create a PayPal app: https://developer.paypal.com/developer/applications/create

        client: {
            //sandbox: 'AZDxjDScFpQtjWTOUtWKbyN_bDt4OgqaF4eYXlewfBP4-8aqX3PiV8e1GWU6liB2CUXlkA59kJXE7M6R',
            sandbox:
                'AeXZWqBXt7O1yxFuharPp_NgVbiGWUd2BQgqMiFghdYFGmn_0CZKurlj8_YRFKG-wKwZcNZh0Ah0ZE6s' //change to use value from control
        },

        payment: function(data, actions) {
            return actions.payment.create({
                payment: {
                    transactions: [
                        {
                            amount: { total: '0.01', currency: 'USD' }
                        }
                    ]
                }
            });
        },

        onAuthorize: function(data, actions) {
            return actions.payment.execute().then(function() {
                location.href = "PayPalCreatePaymentReturn" +
                    '?intent=' +
                    data.intent +
                    '&orderId=' +
                    data.orderID +
                    '&paymentID=' +
                    data.paymentID +
                    '&payerID=' +
                    data.payerID +
                    '&returnUrl=' +
                    data.returnUrl;
            });
        },
        onCancel: function(data) {
            location.href = "PayPalCreatePaymentCancel" +
                '?intent=' +
                data.intent +
                '&paymentID=' +
                data.paymentID +
                '&payerID=' +
                data.payerID +
                '&billingId=' +
                data.billingId +
                '&cancelUrl=' +
                data.cancelUrl +
                '&token=' +
                data.paymentToken;
        },
        onError: function(data) {
            location.href = 'Url.Action("PayPalError")' +
                '?name=' +
                data.name +
                '&message=' +
                data.message;
        }

    }, '#StackedPayPalBtn');

paypal.Button.render({
    env: 'sandbox', // Or 'sandbox',
    commit: true, // Show a 'Pay Now' button
    client: {
        sandbox: 'AeXZWqBXt7O1yxFuharPp_NgVbiGWUd2BQgqMiFghdYFGmn_0CZKurlj8_YRFKG-wKwZcNZh0Ah0ZE6s' //change to use value from control

    },
    style: {
        label: 'buynow',
        color: 'gold',
        size: 'responsive',
        shape: 'pill',
        tagline: false,
        fundingicons: true
    },
    funding: {
        allowed: [paypal.FUNDING.CARD, paypal.FUNDING.CREDIT]
    },
    payment: function (data, actions) {
        return actions.payment.create({
            payment: {
                transactions: [
                    {
                        amount: { total: '1.01', currency: 'USD' }
                    }
                ]
            }
        });
    },
    onAuthorize: function (data, actions) {
        return actions.payment.execute().then(function (payment) {
            location.href = "PayPalCreatePaymentReturn" +
                '?intent=' + data.intent +
                '&orderId=' + data.orderID +
                '&paymentID=' + data.paymentID +
                '&payerID=' + data.payerID +
                '&returnUrl=' + data.returnUrl;
        });
    },
    onCancel: function (data, actions) {
        /*
            * Buyer cancelled the payment
            */
        location.href = "PayPalCreatePaymentCancel" +
            '?intent=' + data.intent +
            '&paymentID=' + data.paymentID +
            '&payerID=' + data.payerID +
            '&billingId=' + data.billingId +
            '&cancelUrl=' + data.cancelUrl +
            '&token=' + data.paymentToken;

    },
    onError: function (err) {
        /*
            * An error occurred during the transaction
            */
        location.href = 'Url.Action("PayPalError")' +
            '?name=' + data.name +
            '&message=' + data.message;
    }
}, '#HorizontalPayPalBtn');

