function FilmAdminDetails(film) {
    $("#tittelFelt").text(film.Title);
    $("#beskrivelse").text(film.Description);
    var pris = "Prisklasse i database: " + film.FilmPriceClassId;

    if (film.Active === true) {
        $("#aktivDB").text("Aktiv i database: ja");
    } else {
        $("#aktivDB").html("Aktiv i database: nei");
    }

    $("#modalPris").text(pris);
    $("#adminBilde").attr("src", film.ImgURL);
    var sjangere = "Filmen er i følgende sjangere: ";
    film.CurrentGenres.forEach(function (element) {
        sjangere += element.Name + ", ";
    });
    sjangere = sjangere.slice(0, -2);
    $("#modalSjangere").text(sjangere);
}