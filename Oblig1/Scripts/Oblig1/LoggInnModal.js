/**
 * Knyttet til LoggInnModal.cshtml, tar imot eventuelle feilmeldinger
 * og viser dem i partialviewet LoggInnModal.
 */
function LogOnModal(resultat) {
    var melding;
    try {
        melding = JSON.parse(resultat);
    } catch (feil) {
        melding = null;
    }
    $("#epostmelding").empty();
    $("#passordmelding").empty();
    if (melding != null) {
        if (melding == "admin") {
            //sets the admin.frontpage
            window.location.replace(AppUrl.Admin);
        } else {
            if (melding == "Oppgi epostadresse") {
                $("#epostmelding").append(melding);
            } else {
                $("#passordmelding").append(melding);
            }
        }
    } else {
        $("#loggInnModal").modal("hide");
        window.location.reload();

        console.log();
    }
}

$("#loggInnModal").on("shown.bs.modal", function () {
    $("#epostFelt").trigger("focus");
});