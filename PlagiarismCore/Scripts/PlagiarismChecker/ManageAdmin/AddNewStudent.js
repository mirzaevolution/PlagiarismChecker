let Ajax = {
    LoadClasses:function(){
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
                }
                

            },
            error: function () {
                console.log(".....Data failed to receive from " + url);
            }
        })
    }
}

$(document).ready(function(){
    Ajax.LoadClasses();
});