let MessageBox = {
    Show: function (text) {
        $("#ModalInformationBody").text(text);
        $("#ModalInformation").modal({ backdrop: "static" });
    }
}
