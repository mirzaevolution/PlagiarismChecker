let MessageBox = {
    Show: function (text) {
        $("#ModalInformationBody").text(text);
        $("#ModalInformation").modal({ backdrop: "static" });
    },
    ShowDark: function (text) {
        
        $("#ModalInformationDarkBody").text(text);
        $("#ModalInformationDark").modal({ backdrop: "static" });
    }
}
