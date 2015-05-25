var rooms = [];

$(document).ready(function () {

    $.connection.hub.start();

    var chat = $.connection.chat;

    $('#send-message').click(function () {

        var msg = $('#message').val();

        chat.server.sendMessage(msg);
    });

    $('#send-anonymous-message').click(function () {

        var msg = $('#anonymous-message').val();
        var userName = $('#userName').val();

        chat.server.sendAnonymousMessage(msg, userName);
    });

    $("#join-room").click(function () {

        var room = $('#room').val();

        chat.server.joinRoom(room)
    });

    $('#send-message-to-room').click(function () {

        var msg = $('#room-message').val();

        chat.server.sendMessageToRoom(msg, rooms);
    });

    $('#logOutButton').click(function () {
        chat.server.removeUserOnLogOut();
    });

    $('#onlineUsers').click(function () {
        chat.server.getOnlineUsers();
    });

    $('#globalMesseges').click(function () {
        //chat.server.getListOfComments();
        chat.server.getNewCommentsCount();
    });

    // Get postId before modal window changes
    $('#globalMesseges').on("customRefreshEvent", function (event, postId) {
        chat.server.refreshCommentsCount(postId);
    });

    // Refresh participant unread messages count before modal window changes
    $('#globalPrivateMessages').on("refreshMessagesEvent", function (event, userName) {
        chat.server.refreshUnreadMessagesCount(userName);
    });

    $('#newPostAlerts').on("refresh", function (event) {
        chat.server.sendNewPostAlert();
    });

    chat.client.addMessage = addMessage;
    chat.client.joinRoom = joinRoom;
    chat.client.updateOnlineUsersCount = updateUsersOnline;
    chat.client.listOfUsersOnline = showListOfOnlineUsers;

    chat.client.updateCommentsCounter = newCommentsCounter;
    chat.client.displayListOfComments = displayListOfComments;
    chat.client.displayNewPostMessage = displayNewPostMessage;
    chat.client.updateUnreadMessagesCounter = updateUnreadMessagesCounter;
});

function displayNewPostMessage(author, postId, title) {
    var messageId = "messageId" + postId;
    //$('#newPostAlerts').append("<div id='" + messageId + "' data-dismiss='alert' class='alert alert-dismissable alert-info hiddenMessage'><button type='button' class='close' data-dismiss='alert'>×</button><strong>" + author + " added new post: <a title='Read Post' data-ajax='true' data-ajax-loading='#mainLoadingElement' data-ajax-method='Get' data-ajax-mode='replace-with' data-ajax-update='#content' href='/Posts/Home/DisplayPost/" + postId + "' id='linkId" + postId + "'> post</a></strong></div>");

    var href = '/Posts/Home/Post/' + postId
    //$a = $('<a></a>');
    //$a.attr('href', href);
    //$a.text('post');

    $('#newPostAlerts').append('<div id="' + messageId + '" class="alert alert-info alert-dismissible hiddenMessage" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>' + author + ' added new post <strong><a href="' + href + '">' + title + ' </a></strong></div>');

    //$('#newPostAlerts').append('<div id="' + messageId + '" class="alert alert-info alert-dismissible hiddenMessage" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>Added new <strong><a title="Read Post" data-ajax="true" data-ajax-loading="#mainLoadingElement" data-ajax-method="Get" data-ajax-mode="replace-with" data-ajax-update="#content" href="/Posts/Home/DisplayPost/' + postId + '" id="linkId' + postId + '"> post</a></strong></div>');

    $('#' + messageId).slideDown(4000, function () {
        //$('#' + messageId).slideUp(5000);
    });
}

function displayListOfComments(comments) {
    console.log('c');
    $ul = $('<ul></ul>');
    $ul.attr("id", "listId");
    for (var i = 0; i < comments.length; i++) {
        var href = '/Posts/Home/DisplayPost?id=' + comments[i].PostId;
        $a = $('<a></a>');
        $a.attr('href', href);
        
        $img = $('<img>');
        $img.attr('src', comments[i].PictureUrl);
        $img.attr('class', 'img-rounded img-responsive commentsUrl');
        $img.attr('alt', 'user pic');

        $span = $('<span></span>');
        $span.text(comments[i].IsReadByAuthor);

        $a.append($img)
        $a.append($span)

        $li = $('<li></li>');
        $li.html($a);
        $ul.append($li)
    }
    // TODO Dropdown ul
    $link = $("#commentsLink");
    $link.attr('data-content', $ul);

    $testUl = $("#testUl");
    $testUl.append($ul);
    console.log($ul);
}

function newCommentsCounter(commentsCount) {
    $counter = $('#globalMessegesCount');
    if (commentsCount > 0) {
        $counter.html(commentsCount);
        $counter.attr("style", "display: initial");
    } else {
        $counter.empty();
        $counter.attr("style", "display: none");
    }
}

function updateUnreadMessagesCounter(unreadMessages) {
    $counter = $('#globalPrivateMessagesCount');
    if (unreadMessages > 0) {
        $counter.html(unreadMessages);
        $counter.attr("style", "display: initial");
    } else {
        $counter.empty();
        $counter.attr("style", "display: none");
    }
}

function showListOfOnlineUsers(onlineUsers) {
    $ul = $("<ul></ul>");
    for (var i = 0; i < onlineUsers.length; i++) {
        var href = '/Home/UserInfo?username=' + onlineUsers[i];
        $a = $('<a></a>');
        $a.attr('href', href);

        $span = $('<span></span>');
        $span.attr('class', 'glyphicon glyphicon-user');
        $span.attr('aria-hidden', 'true');
        $span.text(onlineUsers[i]);

        $a.html($span)

        $li = $('<li></li>');
        $li.html($a);
        $ul.append($li)
    }

    $div = $('<div></div>');
    $div.attr('class', 'modal-content');
    $div.append($ul);
    $('#modalContentElement').html($div);
}

function updateUsersOnline(usersCount) {
    $('#usersOnline').html(usersCount);

    // TODO implement messeges
    $('#unreadMessagesCount').html(usersCount);
}

function addMessage(message) {
    $('#messages').append('<div>' + message + '</div>');
}

function joinRoom(room) {
    rooms.push(room);
    $('#currentRooms').append('<div>' + room + '</div>');
}

(function ($, window, undefined) {
    /// <param name="$" type="jQuery" />
    "use strict";

    if (typeof ($.signalR) !== "function") {
        throw new Error("SignalR: SignalR is not loaded. Please ensure jquery.signalR-x.js is referenced before ~/signalr/js.");
    }

    var signalR = $.signalR;

    function makeProxyCallback(hub, callback) {
        return function () {
            // Call the client hub method
            callback.apply(hub, $.makeArray(arguments));
        };
    }

    function registerHubProxies(instance, shouldSubscribe) {
        var key, hub, memberKey, memberValue, subscriptionMethod;

        for (key in instance) {
            if (instance.hasOwnProperty(key)) {
                hub = instance[key];

                if (!(hub.hubName)) {
                    // Not a client hub
                    continue;
                }

                if (shouldSubscribe) {
                    // We want to subscribe to the hub events
                    subscriptionMethod = hub.on;
                } else {
                    // We want to unsubscribe from the hub events
                    subscriptionMethod = hub.off;
                }

                // Loop through all members on the hub and find client hub functions to subscribe/unsubscribe
                for (memberKey in hub.client) {
                    if (hub.client.hasOwnProperty(memberKey)) {
                        memberValue = hub.client[memberKey];

                        if (!$.isFunction(memberValue)) {
                            // Not a client hub function
                            continue;
                        }

                        subscriptionMethod.call(hub, memberKey, makeProxyCallback(hub, memberValue));
                    }
                }
            }
        }
    }

    $.hubConnection.prototype.createHubProxies = function () {
        var proxies = {};
        this.starting(function () {
            // Register the hub proxies as subscribed
            // (instance, shouldSubscribe)
            registerHubProxies(proxies, true);

            this._registerSubscribedHubs();
        }).disconnected(function () {
            // Unsubscribe all hub proxies when we "disconnect".  This is to ensure that we do not re-add functional call backs.
            // (instance, shouldSubscribe)
            registerHubProxies(proxies, false);
        });

        proxies['chat'] = this.createHubProxy('chat');
        proxies['chat'].client = {};
        proxies['chat'].server = {
            joinRoom: function (room) {
                return proxies['chat'].invoke.apply(proxies['chat'], $.merge(["JoinRoom"], $.makeArray(arguments)));
            },

            sendMessage: function (message) {
                return proxies['chat'].invoke.apply(proxies['chat'], $.merge(["SendMessage"], $.makeArray(arguments)));
            },

            sendMessageToRoom: function (message, rooms) {
                return proxies['chat'].invoke.apply(proxies['chat'], $.merge(["SendMessageToRoom"], $.makeArray(arguments)));
            },

            removeUserOnLogOut: function (message, rooms) {
                return proxies['chat'].invoke.apply(proxies['chat'], $.merge(["RemoveUserOnLogOut"], $.makeArray(arguments)));
            },

            getOnlineUsers: function (message, rooms) {
                return proxies['chat'].invoke.apply(proxies['chat'], $.merge(["GetOnlineUsers"], $.makeArray(arguments)));
            },

            sendAnonymousMessage: function (message, rooms) {
                return proxies['chat'].invoke.apply(proxies['chat'], $.merge(["SendAnonymousMessage"], $.makeArray(arguments)));
            },

            getNewCommentsCount: function () {
                return proxies['chat'].invoke.apply(proxies['chat'], $.merge(["GetNewCommentsCount"], $.makeArray(arguments)));
            },

            updateUnreadMessagesCounter: function () {
                return proxies['chat'].invoke.apply(proxies['chat'], $.merge(["UpdateUnreadMessagesCounter"], $.makeArray(arguments)));
            },

            getListOfComments: function () {
                return proxies['chat'].invoke.apply(proxies['chat'], $.merge(["GetListOfComments"], $.makeArray(arguments)));
            },

            refreshCommentsCount: function (postId) {
                return proxies['chat'].invoke.apply(proxies['chat'], $.merge(["RefreshCommentsCount"], $.makeArray(arguments)));
            },

            refreshUnreadMessagesCount: function (userName) {
                return proxies['chat'].invoke.apply(proxies['chat'], $.merge(["RefreshUnreadMessagesCount"], $.makeArray(arguments)));
            },

            sendNewPostAlert: function () {
                return proxies['chat'].invoke.apply(proxies['chat'], $.merge(["SendNewPostAlert"], $.makeArray(arguments)));
            },
        };

        return proxies;
    };

    signalR.hub = $.hubConnection("/signalr", { useDefaultPath: false });
    $.extend(signalR, signalR.hub.createHubProxies());

}(window.jQuery, window));

// Bootstrap popovers enable
$(function () {
    $('[data-toggle="popover"]').popover();
})