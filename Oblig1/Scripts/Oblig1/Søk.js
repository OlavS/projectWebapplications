/**
 * Skript som håndterer at søkestrengen i søkeboksen blir hentet for hvert trykk og henter da
 * filmkontroller metoden Search. Der blir en ny liste med filmer hentet og et nytt partialview lagd.
 * Dette Partial viewet plasseres i div id = "layoutContainer" i Layout.
 * */

var ajaxKjører = null;

$(document).ready(function () {
    $("#søkeFelt").keyup(function () {
        //Avslutter forrige Ajax kall
        if (ajaxKjører != null) {
            ajaxKjører.abort();
            ajaxKjører = null;
        }

        var streng = $("#søkeFelt").val();
        ajaxKjører = $.ajax({
            url: AppUrl.Søk,
            type: "GET",
            data: { searchString: streng }
        })
            .done(function (partialView) {
                $("#banner").empty();
                $("#layoutContainer").html(partialView);
                //Metode fra Lister og filmeid
                filmThumbEid();
                filmThumbHandlekurv();
            });
    });
});