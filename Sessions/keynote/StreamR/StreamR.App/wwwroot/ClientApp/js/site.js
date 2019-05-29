let connection;

async function startAsync() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/stream", { /*logMessageContent: true,*/ logger: signalR.LogLevel.Information })
        .build();

    let closed = true;
    let isStreaming = false;
    let facingMode = "user";
    let subject;
    connection.onclose(function (err) {
        closed = true;
        if (isStreaming === true) {
            subject.complete();
            camera.stop();
            isStreaming = false;
        }
    });

    connection.on("NewStream", function (name) {
        addStream(name);
    });

    connection.on("RemoveStream", function (name) {
        removeStream(name);
    });

    await connection.start();

    const streams = await connection.invoke("ListStreams");
    if (streams.length > 0) {
        for (let i = 0; i < streams.length; i++) {
            addStream(streams[i]);
        }
    }

    const startStreamButton = document.getElementById('startStream');
    const stopStreamButton = document.getElementById('stopStream');
    const swapCameraButton = document.getElementById('swapCamera');
    const asciiCanvas = document.getElementById("myPre");
    
    stopStreamButton.onclick = function () {
        stopStreamButton.setAttribute("disabled", "disabled");
        swapCameraButton.setAttribute("disabled", "disabled");
        startStreamButton.removeAttribute("disabled");
        if (isStreaming === true) {
            subject.complete();
            camera.stop();
            isStreaming = false;
		}
		removeStream(streamName);
    }
    startStreamButton.onclick = async function () {
        if (connection.connectionState === 0) {
            return;
        }
       
        startStreamButton.setAttribute("disabled", "disabled");
        stopStreamButton.removeAttribute("disabled");
        swapCameraButton.removeAttribute("disabled");
        let streamName = document.getElementById("streamName").value;

        subject = new signalR.Subject();

        startCamera();

        if (streamName === "") {
            streamName = "#" + Math.trunc(Math.random() * 1000);
        }
		await connection.send("StartStream", streamName, subject);
		addStream(streamName);
    }

    swapCameraButton.onclick = async function () {
        if (facingMode === "user") {
            facingMode = "evironment";
        } else {
            facingMode = "user";
        }

        startCamera();
    };

    function startCamera() {
        camera.init({
            width: 200,
            height: 113,
            fps: 30,
            mirror: true,
            facingMode,
            targetCanvas: document.getElementById("myCanvas"),

            onFrame: function (canvas) {
                ascii.fromCanvas(canvas, {
                    //contrast: 170,
                    callback: function (asciiString) {
                        subject.next(asciiString);
						asciiCanvas.innerHTML = asciiString;
						asciiCanvas.style.backgroundColor = "black";
                    }
                });
            },

            onSuccess: function () {
                document.getElementById("myCanvas").style.display = "inline-block";
                isStreaming = true;
            },

            onError: function (error) {
                console.log(error);
            }
        });
    }
}

startAsync();

async function watchStream(streamName) {
    const watchBtn = document.getElementById("button " + streamName);
    watchBtn.setAttribute("disabled", "disabled");
    const closeBtn = document.getElementById("button close " + streamName);
    closeBtn.removeAttribute("disabled");

    const feed = document.getElementById("myPre");

    const ISub = connection.stream("WatchStream", streamName).subscribe({
        next: (item) => {
            feed.innerHTML = item;
        },
        complete: () => {
            feed.innerHTML = "Stream completed";
            console.log("Stream is finished.");
        },
        error: (err) => {
            feed.innerHTML = "Stream closed with an error";
            console.log("Failed to watch the stream: " + err);
        },
    });

    closeBtn.onclick = function () {
        ISub.dispose();
        closeBtn.setAttribute("disabled", "disabled");
        watchBtn.removeAttribute("disabled");
	};

	feed.style.backgroundColor = "black";
}

function addStream(streamName) {
    const list = document.getElementById('streamList');
    const listBody = list.getElementsByTagName('ul')[0];
    if (listBody.children[0].innerText = "No streams available") {
        listBody.innerHTML = '';
    }

    const container = document.createElement('div');

    const strmButton = document.createElement('input');
    strmButton.id = "button " + streamName;
    strmButton.type = "button";
    strmButton.value = "📷";
	strmButton.onclick = function () {
		watchStream(streamName);
	};

    const strmEndButton = document.createElement('input');
    strmEndButton.id = "button close " + streamName;
    strmEndButton.type = "button";
    strmEndButton.value = "🚫";
    strmEndButton.setAttribute("disabled", "disabled");

    const label = document.createElement('span');
    label.innerHTML = streamName;

    const elem = document.createElement('li');
    elem.id = "li " + streamName;
    container.appendChild(label);
    container.appendChild(strmButton);
    container.appendChild(strmEndButton);
    elem.appendChild(container);
    listBody.appendChild(elem);
}

function removeStream(streamName) {
    const list = document.getElementById('streamList');
    const listBody = list.getElementsByTagName('ul')[0];
    if (listBody.children.length === 1) {
        listBody.innerHTML = '';
        const elem = document.createElement('li');
        elem.innerHTML = "<i>No streams available</i>";
        listBody.appendChild(elem);
        return;
    }

    const li = document.getElementById("li " + streamName);
    listBody.removeChild(li);
}