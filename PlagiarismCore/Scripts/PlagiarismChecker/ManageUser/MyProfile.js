let Page = {
    Init: function () {
        this.CheckMessage();
    },
    CheckMessage: function () {
        var message = $("#Message").val();
        if (message && message !== "") {
            alert(message);
        }
    }
}
$(document).ready(function () {
    Page.Init();
})