$(function () {
    $(".ux-new-chore-button").click(function (e) {
        e.preventDefault();

        var template = $(".ux-new-chore-template").clone();
        template.removeClass();
        template.addClass("ux-new-chore");
        $(this).before(template);

        $(".ux-submit-new-chores").removeClass("disabled");
    });

    $(".ux-submit-new-chores").click(function(e) {
        e.preventDefault();

        var chores = [];
        $(".ux-new-chore").each(function() {
            var chore = {
                name: $(this).find('input[name="name"]').val(),
                status: $(this).find('input[name="status"]').prop("checked")
            };
            chores.push(chore);
        });

        $.ajax({
            context: this,
            type: "POST",
            url: "/Home/AddChores/",
            data: JSON.stringify({ chores }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.Success) {
                    location.href = location.href;
                }
            }
        });
    });
});