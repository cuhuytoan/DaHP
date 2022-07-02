//Init dropify.min.js (upload image)
$(function () {
    // Basic
    $('.dropify').dropify();
    // Translated
    $('.dropify-fr').dropify({
        messages: {
            default: 'Glissez-déposez un fichier ici ou cliquez',
            replace: 'Glissez-déposez un fichier ou cliquez pour remplacer',
            remove: 'Supprimer',
            error: 'Désolé, le fichier trop volumineux'
        }
    });
    // Used events
    var drEvent = $('#input-file-events').dropify();
    drEvent.on('dropify.beforeClear', function (event, element) {
        return confirm("Do you really want to delete \"" + element.file.name + "\" ?");
    });
    drEvent.on('dropify.afterClear', function (event, element) {
        alert('File deleted');
    });
    drEvent.on('dropify.errors', function (event, element) {
        console.log('Has Errors');
    });
    var drDestroy = $('#input-file-to-destroy').dropify();
    drDestroy = drDestroy.data('dropify')
    $('#toggleDropify').on('click', function (e) {
        e.preventDefault();
        if (drDestroy.isDropified()) {
            drDestroy.destroy();
        } else {
            drDestroy.init();
        }
    })
});

//sync owl function 
//window.InitOwlSync = () => {
//    var sync1 = $("#sync1");
//    var sync2 = $("#sync2");

//    sync1.owlCarousel({
//        singleItem: true,
//        slideSpeed: 1000,
//        navigation: true,
//        pagination: false,
//        afterAction: syncPosition,
//        responsiveRefreshRate: 200,
//    });

//    sync2.owlCarousel({
//        items: 15,
//        itemsDesktop: [1199, 10],
//        itemsDesktopSmall: [979, 10],
//        itemsTablet: [768, 8],
//        itemsMobile: [479, 4],
//        pagination: false,
//        responsiveRefreshRate: 100,
//        afterInit: function (el) {
//            el.find(".owl-item").eq(0).addClass("synced");
//        }
//    });

//    function syncPosition(el) {
//        var current = this.currentItem;
//        $("#sync2")
//            .find(".owl-item")
//            .removeClass("synced")
//            .eq(current)
//            .addClass("synced")
//        if ($("#sync2").data("owlCarousel") !== undefined) {
//            center(current)
//        }
//    }

//    $("#sync2").on("click", ".owl-item", function (e) {
//        e.preventDefault();
//        var number = $(this).data("owlItem");
//        sync1.trigger("owl.goTo", number);
//    });

//    function center(number) {
//        var sync2visible = sync2.data("owlCarousel").owl.visibleItems;
//        var num = number;
//        var found = false;
//        for (var i in sync2visible) {
//            if (num === sync2visible[i]) {
//                var found = true;
//            }
//        }

//        if (found === false) {
//            if (num > sync2visible[sync2visible.length - 1]) {
//                sync2.trigger("owl.goTo", num - sync2visible.length + 2)
//            } else {
//                if (num - 1 === -1) {
//                    num = 0;
//                }
//                sync2.trigger("owl.goTo", num);
//            }
//        } else if (num === sync2visible[sync2visible.length - 1]) {
//            sync2.trigger("owl.goTo", sync2visible[1])
//        } else if (num === sync2visible[0]) {
//            sync2.trigger("owl.goTo", num - 1)
//        }

//    }

//};