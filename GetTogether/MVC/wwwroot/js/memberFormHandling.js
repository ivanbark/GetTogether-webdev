// ajax call to get current participants of a group
function getMembers(groupid) {
    $("#currentMembers").empty();
    const form = document.forms["memberForm"];
    $.ajax({
        type: "GET",
        url: "https://localhost:7270/api/Groups/getMembers/" + groupid,
        dataType: "json",
        success: function (data) {
            for (let i = 0; i < data.length; i++) {
                var rows = "<tr>" +
                    "<td>" + data[i].name + "</td>" +
                    "<td>" + data[i].email + "</td>" +
                    "<td><button onclick='removeMembers(\"" + data[i].id + "\"," + form.elements["groupid"].value + ")' >Remove</button></td>";
                $('#currentMembers').append(rows);
            } //End of foreach Loop   
            console.log(data);
            var rowCount = $('#currentMembers tr').length + 1;
            $('#memberCount').text("Members: " + rowCount);
        }, //End of AJAX Success function  

        failure: function (data) {
            alert(data.responseText);
        }, //End of AJAX failure function  
        error: function (data) {
            alert(data.responseText);
        } //End of AJAX error function
    });
}

function getUsers() {
    $('#users').empty();
    const form = document.forms["memberForm"];
    $.ajax({
        type: "GET",
        url: "https://localhost:7270/api/Groups/getUsers/" + form.elements["groupid"].value + "/" + form.elements["searchstring"].value,
        dataType: "json",
        success: function (data) {
            if (data.length <= 0) {
                var rows = "<tr>" +
                    "<td>No results found!</td>";
                $('#currentMembers').append(rows);
            } else {
                for (let i = 0; i < data.length; i++) {
                    var rows = "<tr>" +
                        "<td>" + data[i].name + "</td>" +
                        "<td>" + data[i].email + "</td>" +
                        "<td><button onclick='addMembers(\"" + data[i].id + "\"," + form.elements["groupid"].value + ")'>Add</button></td>";
                    $('#users').append(rows);
                } //End of foreach Loop   
                console.log(data);
            }
        }, //End of AJAX Success function  

        failure: function (data) {
            alert(data.responseText);
        }, //End of AJAX failure function  
        error: function (data) {
            alert(data.responseText);
        } //End of AJAX error function
    });
}

document.forms["memberForm"].addEventListener("submit", function (e) {
    e.preventDefault();
    getUsers();
});

function addMembers(userid, groupid) {
    $.ajax({
        type: "PUT",
        url: "https://localhost:7270/api/Groups/addMember/" + userid + "/" + groupid,
        dataType: "json",
        success: function (data) {
            getMembers(groupid);
            getUsers();
        }, //End of AJAX Success function  

        failure: function (data) {
            alert(data.responseText);
        }, //End of AJAX failure function  
        error: function (data) {
            alert(data.responseText);
        } //End of AJAX error function
    });
}

function removeMembers(userid, groupid) {
    $.ajax({
        type: "DELETE",
        url: "https://localhost:7270/api/Groups/removeMember/" + userid + "/" + groupid,
        dataType: "json",
        success: function (data) {
            getMembers(groupid);
            getUsers();
        }, //End of AJAX Success function  

        failure: function (data) {
            alert(data.responseText);
        }, //End of AJAX failure function  
        error: function (data) {
            alert(data.responseText);
        } //End of AJAX error function
    });
}