
const firebaseConfig = {
    apiKey: "AIzaSyAS9YXuXCDVe31HXtcX7Ydq2Q4qCTkqMEA",
    authDomain: "mobileservice-a96bb.firebaseapp.com",
    projectId: "mobileservice-a96bb",
    storageBucket: "mobileservice-a96bb.appspot.com",
    messagingSenderId: "568235933867",
    appId: "1:568235933867:web:28945d7d6780ce064db7ee",
    measurementId: "G-0WQ3CYN7T1"
};

// Initialize Firebase
firebase.initializeApp(firebaseConfig);

window.onload = function () {
    render();
}

function render() {
    window.recaptcharVerifier = new firebase.auth.RecaptchaVerifier('recaptcha-container');
    recaptcharVerifier.render();
}

function PhoneAuth() {
    var phonenumber = document.getElementById('number').value
    firebase.auth().signInWithPhoneNumber(phonenumber, this.window.recaptcharVerifier).then(function (confirmationResult) {
        this.window.confirmationResult = confirmationResult;
        coderesult = confirmationResult;
        console.log(coderesult);
        alert("Da gui ma xac thuc");
    }).catch(function (error) {
        alert("Loi")
    });
}

function coderverify() {
    var code = document.getElementById('numbercode').value;
    coderesult.confirm(code).then(function (result) {
        alert("Mess ok");
        var user = result.user;
        console.log(user);
        window.location.href = "~/Views/Oders/Index.cshtml"
    }).catch(function (error) {
        alert("Loi")
    });
}

