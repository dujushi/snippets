// apply click event to all buttons even when they are renderred using ajax call.
$(document).on("click", ".switch", function () {
    var id = $(this).closest(".task_item").data("id");
    var className = "complete";
    var status = $(this).hasClass(className);
    $.ajax({
        context: this,
        type: "POST",
        url: "/Task/StatusSwitch",
        data: { "id": id, "status": status },
        dataType: "json",
        success: function (data) {
            if (data.Success == true) {
                if (status) {
                    $(this).removeClass(className);
                } else {
                    $(this).addClass(className);
                }
            } else {
                alert("System error, try again later.");
            }
        },
        error: function (request, status, err) {
            alert("System error, try again later.");
        }
    });
});

$("#show_more").click(function() {
    $.ajax({
        type: "GET",
        url: "/Task/ShowTask",
        data: { "showAll": true},
        success: function (data) {
            $("#task_container").html(data);
        },
        error: function (request, status, err) {
            alert("System error, try again later.");
        }
    });
});