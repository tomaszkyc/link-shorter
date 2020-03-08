$(function () {
    console.log('Greetings from CreateLinks.js start');

    //events
    $('#short-url-input').change(shortUrlInput_change);
    $('#redirect-url-input').blur(linkInput_blur);
    $('#create-link-form').submit(createLinkForm_submit);





    console.log('Greetings from CreateLinks.js finished');
});


function hasValidationErrors() {

    let errors = $('#asp-validation-errors').find("span");

    if (errors != null && errors.length > 0) {
        return true;
    }
    else {
        return false;
    }
}


function createLinkForm_submit(e) {
    
    
    let createLinkBtn = $('#create-link-btn');
    createLinkBtn.text('Please wait...');
    createLinkBtn.append('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>');
    
}

function shortUrlInput_change(data) {
    let newShortUrl = $(this).val();


    if (!isStringEmpty(newShortUrl)) {
        checkIfShortUrlIsUnique(newShortUrl);
    }
    else {
        hideShortUrlUniqueMessageWarningMessage();
    }

    
}

function linkInput_blur(data) {

    disableSubmitButton();

    //validate short url
    //if short url is valid - then enable submit button
    let shortUrl = $('#short-url-input').val();
    if (!isStringEmpty(shortUrl)) {
        checkIfShortUrlIsUnique(shortUrl);
    }

    

    //for users who don't have permissions to create own links
    if (!hasAccessToCreateOwnLinks()[0]) {
        let redirectUrl = $('#redirect-url-input').val();

        //check validation errors
        if (hasValidationErrors()) {
            
            disableSubmitButton();
        }
        else if (!isStringEmpty(redirectUrl) && !hasValidationErrors()) {
            //check again if short url is valid
            enableSubmitButton();
        }
        else {
            
            disableSubmitButton();

        }
 
    }

}

function enableSubmitButton() {
    $('#create-link-btn').removeAttr('disabled');
}

function disableSubmitButton() {
    $('#create-link-btn').attr('disabled', true);
}

function showShortUrlUniqueWarningMessage() {
    $('#short-url-uniqueness-warning').removeAttr('hidden');
}

function hideShortUrlUniqueMessageWarningMessage() {
    $('#short-url-uniqueness-warning').attr('hidden', true);
}


function checkIfShortUrlIsUnique(shortUrl_) {

    $.ajax({
        url: '/Dashboard/api/v1/links/check-unique',
        data: {
            shortUrl: shortUrl_
        },
        success: function (response) {
            handleLinkUniqueResponse(response);
        }
    });


}

function hasAccessToCreateOwnLinks() {

    let hasAccess = [];
    let flaga = false;

    $.getJSON('/Dashboard/api/v1/links/rights/can-create-own-short-links')
        .done(function (data) {
            hasAccess.push(data);

        });
    return hasAccess;
}


function handleLinkUniqueResponse(response) {

    let redirectUrl = $('#redirect-url-input').val();
    //if is unique
    if (response === true) {
        hideShortUrlUniqueMessageWarningMessage();

        if ( !isStringEmpty(redirectUrl) ) {
            enableSubmitButton();
        }

        
    }
    //if is not unique

    else {
        disableSubmitButton();
        showShortUrlUniqueWarningMessage();
        
    }

}

function isStringEmpty(str) {
    return !(/\S/.test(str));
}