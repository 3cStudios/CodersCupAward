function GetTimezoneOffset() {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
}

function SetModalDraggableAndResizable() {
    $('.modal-content').resizable({
        minHeight: 300,
        minWidth: 300
    });
    $('.modal-dialog').draggable({ disabled: false });

}
function DisableModalDraggable() {
    $('.modal-dialog').draggable({ disabled: true });
}
function DisableModalDraggableByObjectId(objectId) {
    $('#' + objectId).draggable({ disabled: true });
}

function SetPersonDetailCenter() {
    let mainWidth = $("#main").width();
    let mainleft = $("#main").position();
    $('.person-detail-dialog-content').css({ width: (mainWidth + "px") });
    $('.person-detail-dialog').css({ position: 'absolute', left: (mainleft.left + "px") });

}

function SetInlineStatus(id, status, located) {
    $(("#" + id)).text(status);
    if (located) {
        $(("#" + id)).addClass("btn-status-located");
    }
}

function FileSaveAs(fileContent, fileName, mimeType) {
    let fileUrl = "data:" + mimeType + ";base64," + fileContent;
    fetch(fileUrl)
        .then(res => res.blob())
        .then(blob => {
            let link = window.document.createElement('a');
            link.href = window.URL.createObjectURL(blob), { type: mimeType };
            link.download = fileName;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        });
}
function SetDeliveryOptionSelectPeople() {
    $("#DeliveryOptionEveryone")[0].checked = false;
    $("#DeliveryOptionSelectPeople")[0].checked = true;
}

function HideImportDialog() {
    $("#import-dialog").modal("hide");
}

function HideServiceProviderImportDialog() {
    $('#import-dialog-service-provider').modal('hide');

}
function StartPeopleImporter() {
    peopleImporter.openModal();
}


function StartServiceProviderOrganizationImporter() {
    spoImporter.openModal();
}

function StartServiceProviderImporter() {
    serviceProviderImporter.openModal();
}


function ClipboardCopy(text) {
    navigator.clipboard.writeText(text).then(function () {
        alert("Copied to clipboard!");
    })
        .catch(function (error) {
            alert(error);
        });

}

function ChangeDashboardWidgetSize(size) {
    $(".dashboard-widget").css("min-width", (size + "px"));
}

function SetHelpPanelRight() {
    let mainWidth = $("#main").width();
    if (mainWidth < 1000) {
        $('.help-panel').css({ left: 0 });
    } else {
        $('.help-panel').css({ right: 0 }).css('left', 'auto');
    }
}

function SetHelpPanelLeft() {
    $('.help-panel').css({ left: 0 }).css('right', 'auto');
}

function playVideo() {
    var video = document.getElementById('videoPlayer');

    if (video == null) {
        return;
    }

    video.style.display = 'block'; // Show the video
    video.play();
}


function InvalidInputValue(inputId) {
    $("#" + inputId).val('')
        .addClass('is-invalid') // Add the 'invalid' class to the input
        .focus();
    
}

function ValidInputValue(inputId) {
    $("#" + inputId).removeClass('is-invalid');

}