const paymentForm = document.getElementById("paymentForm");
paymentForm.addEventListener("submit", payWithPaystack, false);
function payWithPaystack(e) {
  e.preventDefault();

  let handler = PaystackPop.setup({
    key: "pk_test_fe5a54565a0b6f9a04eb69e5ce395810ea27a2bc", // Replace with your public key
    email: document.getElementById("email-address").value,
    amount: document.getElementById("amount").value * 100,
    ref: "" + Math.floor(Math.random() * 1000000000 + 1), // generates a pseudo-unique reference. Please replace with a reference you generated. Or remove the line entirely so our API will generate one for you
    // label: "Optional string that replaces customer email"
    onClose: function () {
      alert("Window closed.");
    },
    callback: function (response) {
      let message = "Payment complete! Reference: " + response.reference;
      alert(message);
      if (document.getElementById("amount").value == "200") {
        var x = document.getElementById("email-address").value;
        window.location = `https://localhost:7229/Farmer/UpdateToHasPaid?userEmail=${x}`;
      } else {
        window.location =
          "https://localhost:7229/RequestedProduct/UpdateToHasPaid";
      }
    },
  });
  handler.openIframe();
}
