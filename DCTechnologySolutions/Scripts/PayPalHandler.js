
function GetCurrentClientId() {
    console.log("----");
    console.log($("#ClientId").val());
    console.log("----");
    //var cId = $("#UserClientId").val();
    var cId = $("#ClientId").val();
    console.log("#####");
    console.log(cId);
    console.log("#####");
    if (cId === "") {
        cId = $("#ClientId").val();
    }
    console.log("----");
    console.log($("#ClientId").val());
    console.log("----");
    return cId;// "ClientId";
}

function GetCurrentClientSecretKey() {
    return "SecrectKey";
}

var myClientId = GetCurrentClientId();
console.log("++++");
console.log(myClientId);

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
