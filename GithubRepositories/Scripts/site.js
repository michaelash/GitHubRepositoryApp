$('document').ready(function ()
{
    $(document).on("click", ".add-bk", function (e)
    {
        var $btn = $(this);
        $.ajax({
            type: 'POST',
            url: AppSettings.BaseUrl +"Home/AddBookmark",
            dataType: "json",
            data:
            {
                name: $btn.parent().children('.res-name').text(), image: $btn.parent().children('img').attr('src'),
                id: $btn.parent().data('id')
            },
            success: function (result)
            {
                $btn.text("Remove Bookmark");
                $btn.removeClass("add-bk").addClass("del-bk");
            },
            error: function (data, err)
            {
                alert("There was some problem. Please try again later, and if the problem persists, contact the site team.");
            }
        });
    });

    $(document).on("click", ".del-bk", function (e)
    {
        var $btn = $(this);
        $.ajax({
            type: 'POST',
            url: AppSettings.BaseUrl + "Home/RemoveBookmark",
            dataType: "json",
            data:
            {
                id: $btn.parent().data('id')
            },
            success: function (result)
            {
                $btn.text("Add Bookmark");
                $btn.removeClass("del-bk").addClass("add-bk");
            },
            error: function (data, err)
            {
                alert("There was some problem. Please try again later, and if the problem persists, contact the site team.");
            }
        });
    });
});
