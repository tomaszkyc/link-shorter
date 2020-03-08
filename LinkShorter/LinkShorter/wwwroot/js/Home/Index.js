$(function () {
    console.log('Greetings from Index.js start');

    $('#create-short-link-form').submit(createShortLinkForm_submit);
    $('#RedirectUrl').blur(redirectUrl_blur);
    disableButton('shorten-button-action');
    //shorten-button-action



    console.log('Greetings from Index.js finished');
});


function redirectUrl_blur() {

    //check if there are some errors
    let validationErrors = $('#RedirectUrl-error');
    let linkToShorten = $('#RedirectUrl').val();
    //if there are some validation errors
    if (validationErrors != null && validationErrors.length > 0) {
        disableButton('shorten-button-action');
    }
    else if (!isStringEmpty(linkToShorten) ){
        enableButton('shorten-button-action');
    }


}


function createShortLinkForm_submit(e) {


    let shortenBtn = $('#shorten-button-action');
    shortenBtn.text('Please wait... ');
    shortenBtn.append('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>');

}


function disableButton(buttonId) {
    $('#' + buttonId).attr('disabled', true);
}

function enableButton(buttonId) {
    $('#' + buttonId).removeAttr('disabled');
}


function isStringEmpty(str) {
    return !(/\S/.test(str));
}