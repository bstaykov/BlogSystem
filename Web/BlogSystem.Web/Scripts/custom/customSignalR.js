﻿var rooms = [];

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

    chat.client.addMessage = addMessage;
    chat.client.joinRoom = joinRoom;
    chat.client.updateOnlineUsersCount = updateUsersOnline;
    chat.client.listOfUsersOnline = showListOfOnlineUsers;
});

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
    $('#listOfOnlineUsers').html($ul);
}

function updateUsersOnline(usersCount) {
    $('#usersOnline').html(usersCount);
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
            }
            ,

            sendAnonymousMessage: function (message, rooms) {
                return proxies['chat'].invoke.apply(proxies['chat'], $.merge(["SendAnonymousMessage"], $.makeArray(arguments)));
            }
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