$(function () {
    console.log('Greetings from EditUser.js start');

    //events

    $('#btnAllRight').click(btnAllRight_click);
    $('#btnRight').click(btnRight_click);
    $('#btnLeft').click(btnLeft_click);
    $('#btnAllLeft').click(btnAllLeft_click);
    $('#edit-user-form').submit(editUserForm_submit);

    getAvailableRolesForUser();
    getUserActualRoles();



    console.log('Greetings from EditUser.js finished');
});


function btnAllRight_click(e) {
    let selectedOpts = $('#user-available-roles option');
    if (selectedOpts.length == 0) {
        alert("Nothing to move.");
        e.preventDefault();
    }

    $('#user-actual-roles').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();

    //check if should disable button
    EnableDisableButtons();
}

function btnRight_click(e) {
    let selectedOpts = $('#user-available-roles option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move.");
        e.preventDefault();
    }

    $('#user-actual-roles').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();

    //check if should disable button
    EnableDisableButtons();
}

function btnLeft_click(e) {
    let selectedOpts = $('#user-actual-roles option:selected');
    if (selectedOpts.length == 0) {
        alert("Nothing to move.");
        e.preventDefault();
    }

    $('#user-available-roles').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();

    //check if should disable button
    EnableDisableButtons();
}

function btnAllLeft_click(e) {
    let selectedOpts = $('#user-actual-roles option');
    if (selectedOpts.length == 0) {
        alert("Nothing to move.");
        e.preventDefault();
    }

    $('#user-available-roles').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();

    //check if should disable button
    EnableDisableButtons();
}


function getUserActualRoles() {
    $.ajax({
        url: '/Dashboard/User/api/v1/user/roles/actual',
        data: {
            userId: $('#Id').val()
        },
        async: true,
        success: function (response) {
            handleUserActualRolesResponse(response);
        },
        failure: function (response) {
            console.log('data not fetch from API: ' + response);
        }
    });
}

function handleUserActualRolesResponse(response) {

    console.log(response);

    let actualRolesList = $('#user-actual-roles');

    //clear list

    actualRolesList.empty();

    response.forEach(function (role) {
        let singleOption = $('<option>');
        singleOption.val(role.id)
            .text(role.name);

        actualRolesList.append(singleOption);

        console.log(role);
    });
}


function getAvailableRolesForUser() {

    $.ajax({
        url: '/Dashboard/User/api/v1/user/roles/available',
        data: {
            userId: $('#Id').val()
        },
        async: true,
        success: function (response) {
            handleAvailableRolesResponse(response);
        },
        failure: function (response) {
            console.log('data not fetch from API: ' + response);
        }
    });

}

function handleAvailableRolesResponse(response) {
    console.log(response);

    let availableRolesList = $('#user-available-roles');

    //clear list

    availableRolesList.empty();

    response.forEach(function(role) {
        let singleOption = $('<option>');
        singleOption.val(role.id)
            .text(role.name);

        availableRolesList.append(singleOption);

        console.log(role);
    });



}


function EnableDisableButtons() {
    
    let availableRoles = $('#user-available-roles');
    if (availableRoles[0].length == 0) {

        //disable buttons
        disableButton('btnAllRight');
        disableButton('btnRight');
    }
    else {
        //enable buttons
        enableButton('btnAllRight');
        enableButton('btnRight');
    }

    let actualRoles = $('#user-actual-roles');
    if (actualRoles[0].length == 0) {

        //disable buttons
        disableButton('btnAllLeft');
        disableButton('btnLeft');
    }
    else {

        //enable buttons
        enableButton('btnAllLeft');
        enableButton('btnLeft');
    }

}

function disableButton(buttonId) {
    $('#' + buttonId).attr('disabled', true);
}

function enableButton(buttonId) {
    $('#' + buttonId).removeAttr('disabled');
}


function editUserForm_submit(e) {

    e.preventDefault();
    console.log('Form submitted!');

    let editUserBtn = $('#edit-user-btn');
    editUserBtn.text('Saving... ');
    editUserBtn.append('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>');

    //fire functions
    grantUserRoles(this);
    
    
    
}

function grantUserRoles(form) {

    var rolesArr = new Array;

    $("#user-actual-roles option").each(function () {
        rolesArr.push($(this).val());
    });

    $.ajax({
        url: '/Dashboard/User/api/v1/user/roles/edit',
        data: {
            userId: $('#Id').val(),
            rolesToGrant: rolesArr
        },
        success: function (response) {
            console.log('success: ' + response);
            //process form submit
            $(form).unbind('submit').submit();
        },
        failure: function (error) {
            console.log('failure: ' + error);
        },
        method: 'POST'

    });


}