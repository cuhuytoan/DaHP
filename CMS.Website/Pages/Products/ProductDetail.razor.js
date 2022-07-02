window.ShareFunction = {
    fbDialog: function () {
        FB.ui({
            display: 'popup',
            method: 'share',
            href: 'https://developers.facebook.com/docs/',
        }, function (response) { });

    },
    copyToClipBooard: function () {
        navigator.clipboard.writeText(window.location.href);
    }

}