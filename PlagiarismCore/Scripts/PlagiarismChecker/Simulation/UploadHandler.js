let Buttons = {
    Init: function(){
        this.ButtonTokenizing();
        this.ButtonPurifying();
        this.ButtonStopwords();
        this.ButtonStemming();
        this.ButtonSorting();
        this.ButtonAnalyze();
    },
    ButtonTokenizing: function () {
        $("#ButtonTokenizing").click(function () {
            try {
                var isReadyToProcess = $("#IsReadyToProcess").val();
                if (isReadyToProcess === "true") {
                    var sampleText = $("#sampleText").val();
                    var comparerText = $("#comparerText").val();
                    var url = "/Simulation/TokenizingHandler";

                    $.ajax({
                        url: url,
                        beforeSend: function () {
                            console.log(".....Posting data to " + url);
                            $("#ButtonTokenizing").prop("disabled", true);
                        },
                        data: {
                            sample: sampleText,
                            comparer: comparerText
                        },
                        method: "POST",
                        success: function (response) {
                            $("#ButtonTokenizing").prop("disabled", false);
                            if (response) {
                                $("#step").val(response.Step);
                                $("#sampleTokenizingText").val(response.SampleText);
                                $("#comparerTokenizingText").val(response.ComparerText);
                                $("#SampleTokenizing").removeClass("collapse");
                                $("#ComparerTokenizing").removeClass("collapse");
                                MessageBox.ShowDark("Process completed!");
                                window.location.href = "#SampleTokenizing";
                            }
                        },
                        error: function () {
                            $("#ButtonTokenizing").prop("disabled", false);
                            MessageBox.ShowDark("An error occured while posting data to server");
                        }
                    })
                } else {
                    MessageBox.ShowDark("Please upload the required files first!");
                }
            }
            catch (ex) { console.log(ex) }
        });
    },
    ButtonPurifying: function () {
        $("#ButtonPurifying").click(function () {
            try {
                var isReadyToProcess = $("#IsReadyToProcess").val();
                if (isReadyToProcess === "true") {
                    var sampleText = $("#sampleText").val();
                    var comparerText = $("#comparerText").val();
                    var url = "/Simulation/PurifyingHandler";

                    $.ajax({
                        url: url,
                        beforeSend: function () {
                            console.log(".....Posting data to " + url);
                            $("#ButtonPurifying").prop("disabled", true);
                        },
                        data:{
                            sample: sampleText,
                            comparer: comparerText
                        },
                        method: "POST",
                        success: function (response) {
                            $("#ButtonPurifying").prop("disabled", false);
                            if (response) {
                                $("#step").val(response.Step);
                                $("#samplePurifyingText").val(response.SampleText);
                                $("#comparerPurifyingText").val(response.ComparerText);
                                $("#SamplePurifying").removeClass("collapse");
                                $("#ComparerPurifying").removeClass("collapse");
                                MessageBox.ShowDark("Process completed!");
                                window.location.href = "#SamplePurifying";
                            }
                        },
                        error: function () {
                            $("#ButtonPurifying").prop("disabled", false);
                            MessageBox.ShowDark("An error occured while posting data to server");
                        }
                    })
                } else {
                    MessageBox.ShowDark("Please upload the required files first!");
                }
            }
            catch (ex) { console.log(ex) }
        });
    },
    ButtonStopwords: function () {
        $("#ButtonStopwords").click(function () {
            try {
                var isReadyToProcess = $("#IsReadyToProcess").val();
                if (isReadyToProcess === "true") {
                    var sampleText = $("#sampleText").val();
                    var comparerText = $("#comparerText").val();
                    var url = "/Simulation/StopwordsHandler";

                    $.ajax({
                        url: url,
                        beforeSend: function () {
                            console.log(".....Posting data to " + url);
                            $("#ButtonStopwords").prop("disabled", true);
                        },
                        data: {
                            sample: sampleText,
                            comparer: comparerText
                        },
                        method: "POST",
                        success: function (response) {
                            $("#ButtonStopwords").prop("disabled", false);
                            if (response) {
                                $("#step").val(response.Step);
                                $("#sampleStopwordText").val(response.SampleText);
                                $("#comparerStopwordsText").val(response.ComparerText);
                                $("#SampleStopword").removeClass("collapse");
                                $("#ComparerStopword").removeClass("collapse");
                                MessageBox.ShowDark("Process completed!");
                                window.location.href = "#SampleStopword";
                            }
                        },
                        error: function () {
                            $("#ButtonStopwords").prop("disabled", false);
                            MessageBox.ShowDark("An error occured while posting data to server");
                        }
                    })
                } else {
                    MessageBox.ShowDark("Please upload the required files first!");
                }
            }
            catch (ex) { console.log(ex) }
        });
    },
    ButtonStemming: function () {
        $("#ButtonStemming").click(function () {
            try {
                var isReadyToProcess = $("#IsReadyToProcess").val();
                if (isReadyToProcess === "true") {
                    var sampleText = $("#sampleText").val();
                    var comparerText = $("#comparerText").val();
                    var url = "/Simulation/StemmingHandler";

                    $.ajax({
                        url: url,
                        beforeSend: function () {
                            console.log(".....Posting data to " + url);
                            $("#ButtonStemming").prop("disabled", true);
                        },
                        data: {
                            sample: sampleText,
                            comparer: comparerText
                        },
                        method: "POST",
                        success: function (response) {
                            $("#ButtonStemming").prop("disabled", false);
                            if (response) {
                                $("#step").val(response.Step);
                                $("#sampleStemmingText").val(response.SampleText);
                                $("#comparerStemmingText").val(response.ComparerText);
                                $("#SampleStemming").removeClass("collapse");
                                $("#ComparerStemming").removeClass("collapse");
                                MessageBox.ShowDark("Process completed!");
                                window.location.href = "#SampleStemming";
                            }
                        },
                        error: function () {
                            $("#ButtonStemming").prop("disabled", false);
                            MessageBox.ShowDark("An error occured while posting data to server");
                        }
                    })
                } else {
                    MessageBox.ShowDark("Please upload the required files first!");
                }
            }
            catch (ex) { console.log(ex) }
        });
    },
    ButtonSorting: function () {
        $("#ButtonSorting").click(function () {
            try {
                var isReadyToProcess = $("#IsReadyToProcess").val();
                if (isReadyToProcess === "true") {
                    var sampleText = $("#sampleText").val();
                    var comparerText = $("#comparerText").val();
                    var url = "/Simulation/SortingHandler";

                    $.ajax({
                        url: url,
                        beforeSend: function () {
                            console.log(".....Posting data to " + url);
                            $("#ButtonSorting").prop("disabled", true);
                        },
                        data: {
                            sample: sampleText,
                            comparer: comparerText
                        },
                        method: "POST",
                        success: function (response) {
                            $("#ButtonSorting").prop("disabled", false);
                            if (response) {
                                $("#step").val(response.Step);
                                $("#sampleSortingText").val(response.SampleText);
                                $("#comparerSortingText").val(response.ComparerText);
                                $("#SampleSorting").removeClass("collapse");
                                $("#ComparerSorting").removeClass("collapse");
                                MessageBox.ShowDark("Process completed!");
                                window.location.href = "#SampleSorting";
                            }
                        },
                        error: function () {
                            $("#ButtonSorting").prop("disabled", false);
                            MessageBox.ShowDark("An error occured while posting data to server");
                        }
                    })
                } else {
                    MessageBox.ShowDark("Please upload the required files first!");
                }
            }
            catch (ex) { console.log(ex) }
        });
    },
    ButtonAnalyze: function () {
        $("#ButtonAnalyze").click(function () {
            try {
                var isReadyToProcess = $("#IsReadyToProcess").val();
                if (isReadyToProcess === "true") {
                    var sampleText = $("#sampleText").val();
                    var comparerText = $("#comparerText").val();
                    var url = "/Simulation/Check";

                    $.ajax({
                        url: url,
                        beforeSend: function () {
                            console.log(".....Posting data to " + url);
                            $("#ButtonAnalyze").prop("disabled", true);
                        },
                        data: {
                            sample: sampleText,
                            comparer: comparerText
                        },
                        method: "POST",
                        success: function (response) {
                            $("#ButtonAnalyze").prop("disabled", false);
                            if (response) {
                                var description = "";
                                description += "Sample Char Length: " + response.SampleCharCount + "\n";
                                description += "Sample Word Length: " + response.SampleWordCount + "\n";
                                description += "Comparer Char Length: " + response.ComparerCharCount + "\n";
                                description += "Comparer Word Length: " + response.ComparerWordCount + "\n";
                                description += "Description: " + response.Description + "\n";
                                description += "Percentage: " + response.PercentageInteger + "%" + "\n";
                                description += "Max: " + response.Max + "\n";
                                description += "Distance: " + response.Distance + "\n";
                                description += "Calculation Time: " + response.CalculationTime.TotalSeconds + " seconds\n";
                                $("#resultText").val(description);
                                MessageBox.ShowDark("Process completed!");
                                window.location.href = "#resultText";
                            }
                        },
                        error: function () {
                            $("#ButtonAnalyze").prop("disabled", false);
                            MessageBox.ShowDark("An error occured while posting data to server");
                        }
                    })
                } else {
                    MessageBox.ShowDark("Please upload the required files first!");
                }
            }
            catch (ex) { console.log(ex) }
        });
    },
}
$(document).ready(function () {
    Buttons.Init();
});