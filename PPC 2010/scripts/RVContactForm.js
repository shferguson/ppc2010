$(document).ready(function () {
    $("#ContactForm input[type=button]").click(function () {
        $("#ServerMsgContainer").slideUp();
        var validator = $("#ContactForm").validate({
            rules: {
                name: { required: true },
                company: { required: false },
                phone: { required: true, minlength: 7, digits: true },
                email: { required: true, email: true },
                subject: { required: true },
                message: { required: true }
            },
            messages: {
                name: errName,
                phone: {
                    required: errPhone,
                    minlength: errPhone,
                    digits: errDigits
                },
                email: {
                    required: errorEmail,
                    email: errorEmail
                },
                subject: errSubject,
                message: errMessage
            },
            submitHandler: AjaxSubmit
        }).form();
        $("#ContactForm").submit(); //$(this).closest('form').submit();

    });
});

function AjaxSubmit() {
    $("#ContactForm #submit").attr('disabled', 'disabled');
    $("#ContactForm #submit").after('<img src="/media/assets/ajax-loader.gif" class="loader" />');

    // Convert the form data into an js object
    var myMailerRequest = { languageID: languageID, name: $('#name').val(), company: $('#company').val(), phone: $('#phone').val(), email: $('#email').val(), subject: $('#subject').val(), message: $('#message').val() };
    // JSONify the js object - desirialize js object to MailerRequest object. the first param must be equal to the name of the input parameter in the web service method "SendContactForm"
    var data = JSON.stringify({ req: myMailerRequest });

    $.ajax({
        type: "POST",
        url: "/umbraco/webservices/RVContactFormMailer.asmx/SendContactForm",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: AjaxSucceeded,
        error: AjaxFailed
    });
}

function AjaxSucceeded(result) {
    $('#ServerMsgHeader').html(thankYouHeaderText);
    $('#ServerMsgMessage').html(thankYouMessageText);

    $("#ServerMsgContainer").removeClass("emailFailure");
    $("#ServerMsgContainer").addClass("emailSuccess");

    $('#ServerMsgContainer').slideDown('slow');
    $('#ContactForm img.loader').fadeOut('fast', function () { $(this).remove() });
    $("#ContactForm #submit").attr('disabled', '');
    $('#ContactForm').slideUp('slow');
    return
}

function AjaxFailed(result) {
    $("#ServerMsgContainer").removeClass("emailSuccess");
    $("#ServerMsgContainer").addClass("emailFailure");
    $('#ServerMsgHeader').html(failureHeaderText);
    $('#ServerMsgMessage').html(failureMessageText);
    $('#ServerMsgContainer').slideDown('slow');
    $('#ContactForm img.loader').fadeOut('fast', function () { $(this).remove() });
    $("#ContactForm #submit").attr('disabled', '');
    return;
}