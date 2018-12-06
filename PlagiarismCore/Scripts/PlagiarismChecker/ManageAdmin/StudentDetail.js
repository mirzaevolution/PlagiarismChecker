let Ajax = {
    LoadClasses: function () {
        var url = "/CoreAPI/GetClasses";
        $.ajax({
            url: url,
            beforeSend: function () {
                console.log(".....Requesting data from " + url);
            },
            success: function (response) {
                console.log(".....Data successfully retrieved from " + url);
                $("#ClassID").empty();
                if (response.length > 0) {
                    $.each(response, (index, value) => {
                        $("#ClassID").append("<option value='" + value.ID + "'>" + value.Text + "</option>");
                    });
                    $("#ClassID").val($("#ClassIDHidden").val());
                }

            },
            error: function () {
                console.log(".....Data failed to receive from " + url);
            }
        })
    }
}
let Page = {
    CheckMessage: function () {
        var message = $("#Message").val();
        if (message && message !== "") {
            alert(message);
        }
    }
}
let Buttons = {
    Init: function () {
        $("#ButtonDelete").click(function () {
            $("#FormDelete").submit();
        });
    }
}
$(document).ready(function () {
    Buttons.Init();
    Page.CheckMessage();
    Ajax.LoadClasses();
});