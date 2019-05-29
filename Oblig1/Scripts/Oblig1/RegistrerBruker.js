/**
 * Funksjon som trigges når kunden taster inn en epostadresse i Registrering.cshtml.
 * Kunden får en melding dersom epostadressen allerede eksisterer i databasen,
 * samtidig blir submit knappen disablet. Dersom kunden velger å registrere seg
 * med en annen epostadresse, blir meldingen borte og knappen enablet igjen.
 */
function EpostEksisterer() {
    var epost = $("#Email").val();

    $.ajax({
        type: "POST",
        url: AppUrl.SjekkEpost,
        data: JSON.stringify({ email: epost }),
        contentType: "application/json",

        success:
            function (resultat) {
                var funnet = JSON.parse(resultat);
                if (funnet === true) {
                    $("#epostMelding").empty();
                    $("#epostMelding").append("Epostadressen du har oppgitt finnes allerede i databasen.");
                    $("#registrerKnapp").prop("disabled", true);
                } else {
                    $("#epostMelding").empty();
                    $("#registrerKnapp").prop("disabled", false);
                }
            }
    });
};