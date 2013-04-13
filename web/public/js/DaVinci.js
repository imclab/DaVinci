var Davinci = function () {

    var container;
    var nextId = 0;
    var camera, controls, scene, projector, renderer;
    var objects = [], plane;
    var textures = [];
    var width = window.innerWidth,
        height = window.innerHeight;
    var current = null;
    var clock;
    var mouse = new THREE.Vector2(),
    offset = new THREE.Vector3(),INTERSECTED, SELECTED;

   function init(width, height)  {

        container = document.getElementById('container');
        var angle = 60, aspect = width / height, near = 1, far = 20000;
        clock = new THREE.Clock();
        camera = new THREE.PerspectiveCamera(angle, aspect , near, far);
        scene = new THREE.Scene();
        scene.add(camera);
        camera.position.y = 10;
        camera.lookAt(scene.position);

        detect() ? initWebGL(width, height) : initCSS3D(widht, height);


    }

    function detect() {
        return window.WebGLRenderingContext;
    }

    function initWebGL(width, height) {

        renderer = new THREE.WebGLRenderer({ antialias: true });
        renderer.sortObjects = false;
        renderer.setSize(width, height);

        controls = new THREE.FirstPersonControls(camera);
        controls.movementSpeed = 100;
        controls.lookSpeed = 0.125;
        controls.lookVertical = false;
        
        container.appendChild(renderer.domElement);

        var light = new THREE.DirectionalLight(0xffffff, 2);
        light.position.set(1, 1, 1).normalize();
        scene.add(light);

        var light = new THREE.DirectionalLight(0xffffff);
        light.position.set(-1, -1, -1).normalize();
        scene.add(light);

        var floorTexture = new THREE.ImageUtils.loadTexture( 'images/fl.jpg' );
        floorTexture.wrapS = floorTexture.wrapT = THREE.RepeatWrapping;
        floorTexture.repeat.set( 10, 10 );

        var floorMaterial = new THREE.MeshBasicMaterial( { map: floorTexture } );
        var floorGeometry = new THREE.PlaneGeometry(1000, 1000, 10, 10);
        var floor = new THREE.Mesh(floorGeometry, floorMaterial);

        floor.position.y = -40;
        floor.rotation.x = - Math.PI/2
        floor.doubleSided = true;
        scene.add(floor);

        plane = new THREE.Mesh(new THREE.PlaneGeometry(2000, 2000, 8, 8), new THREE.MeshBasicMaterial({ color: 0x000000, opacity: 0.25, transparent: true, wireframe: true }));
        plane.visible = false;
        scene.add(plane);

        projector = new THREE.Projector();

        renderer.domElement.addEventListener('mousemove', onDocumentMouseMove, false);
        renderer.domElement.addEventListener('mousedown', onDocumentMouseDown, false);
        renderer.domElement.addEventListener('mouseup', onDocumentMouseUp, false);

        window.addEventListener('resize', this.onWindowResize, false);
        textures.push('/images/ml.jpg');
        createPainting('/images/ml.jpg', 'this is monalisa');

    }

    function initCSS3D(width, height) {

        renderer = new THREE.CSS3DRenderer();
        renderer.setSize( width, height );
        renderer.domElement.style.position = 'absolute';
        container.appendChild( renderer.domElement );

        controls = new THREE.TrackBallControls(camera, renderer.domElement);
        controls.rotateSpeed = 0.5;

    }


    function onWindowResize () {
        var w = window.innerWidth;
        var h = window.innerHeight;
        camera.aspect = w / h;
        camera.updateProjectionMatrix();

        renderer.setSize(w, h);

    }

   function onDocumentMouseMove(event) {

        event.preventDefault();
        mouse.x = ((event.clientX - renderer.domElement.offsetLeft)/ renderer.domElement.width) * 2 - 1;
        mouse.y = -((event.clientY - renderer.domElement.offsetTop) / renderer.domElement.height) * 2 + 1;

        var vector = new THREE.Vector3(mouse.x, mouse.y, 0.5);
        projector.unprojectVector(vector, camera);

        var ray = new THREE.Raycaster(camera.position, vector.sub(camera.position).normalize());


        if (SELECTED) {
            return;

        }


        var intersects = ray.intersectObjects(objects);

        if (intersects.length > 0) {

            if (INTERSECTED != intersects[0].object) {

                INTERSECTED = intersects[0].object;
                plane.position.copy(INTERSECTED.position);
                plane.lookAt(camera.position);

            }
            if(INTERSECTED != null) {
            $("#" + objects.indexOf(INTERSECTED)).tooltip().tooltip('open');
            }

            container.style.cursor = 'pointer';

        } else {

            if(INTERSECTED != null) {
                $("#" + objects.indexOf(INTERSECTED)).tooltip('close');
            }
            INTERSECTED = null;
            container.style.cursor = 'auto';

        }

    }

    function onDocumentMouseDown(event) {

        event.preventDefault();

        var vector = new THREE.Vector3(mouse.x, mouse.y, 0.5);
        projector.unprojectVector(vector, camera);

        var ray = new THREE.Raycaster(camera.position, vector.sub(camera.position).normalize());

        var intersects = ray.intersectObjects(objects);

        if (intersects.length > 0) {

            controls.enable = false;

            SELECTED = intersects[0].object;
            zoomImage('/images/ml.jpg');
            current = SELECTED;

        }

    }
    function setEnable() {

        controls.activeLook = true;
        if (INTERSECTED) {
            SELECTED = null;
        }
        container.style.cursor = 'auto';
    }

    function onDocumentMouseUp(event) {

        event.preventDefault();
        container.style.cursor = 'auto';

    }

    function animate() {

        requestAnimationFrame(animate);

        render();

    }

    function render() {
        var delta = clock.getDelta();
        controls.update(delta);

        renderer.render(scene, camera);
    }

    function createPainting(texUrl, tooltip) {

        var materialArray = [];
        materialArray.push(new THREE.MeshBasicMaterial( { color: 0xc0c0c0 }));
        materialArray.push(new THREE.MeshBasicMaterial( { color: 0xc0c0c0 }));
        materialArray.push(new THREE.MeshBasicMaterial( { color: 0xc0c0c0 }));
        materialArray.push(new THREE.MeshBasicMaterial( { color: 0xc0c0c0 }));
        materialArray.push(new THREE.MeshBasicMaterial( { map: THREE.ImageUtils.loadTexture( texUrl )} ));
        materialArray.push(new THREE.MeshBasicMaterial( { color: 0xc0c0c0 }));

        var cubeGeometry = new THREE.CubeGeometry( 85, 85, 1 , 1, 1, 1 );
        var painting = new THREE.Mesh(cubeGeometry,new THREE.MeshFaceMaterial( materialArray ));
        painting.position.set(0, 0, 0);
        //var painting = new Painting(nextId);
        //painting.createObject(texUrl);
        scene.add(painting);
        nextId++;
        objects.push(painting);
        current = painting;
        generateTooltip(current, tooltip);


    }
    function generateTooltip(currentObject, tooltip) {

        $("#container").append('<div id=' + objects.indexOf(currentObject) + ' title=' + '"' + tooltip + '"' + "></div>");

    }

    function zoomImage(uri) {

        imageDialog = $("#dialog");
        imageTag = $('#myimage');
        controls.activeLook = false;
        imageTag.one("load", function (){
        }).attr('src', uri);
        imageDialog.dialog('open');
    }

    function loadJSONObj(roomName) {

        var loader = new THREE.JSONLoader();

        loader.load('models/test.js', function ( geometry ) {
            var room = new THREE.Mesh( geometry, new THREE.MeshNormalMaterial() );
            room.scale.set(10,10,10);
            room.position.x = 0;
            room.position.y = 0;
            room.position.z = 0;
            scene.add(room);

        });
    }
    return {
        init: init,
        animate: animate,
        createPainting: createPainting,
        setEnable: setEnable
    };

}();

function dec2hex(i) {
  var result = "0x000000";
  if (i >= 0 && i <= 15) { result = "0x00000" + i.toString(16); }
  else if (i >= 16 && i <= 255) { result = "0x0000" + i.toString(16); }
  else if (i >= 256 && i <= 4095) { result = "0x000" + i.toString(16); }
  else if (i >= 4096 && i <= 65535) { result = "0x00" + i.toString(16); }
  else if (i >= 65535 && i <= 1048575) { result = "0x0" + i.toString(16); }
  else if (i >= 1048575 ) { result = '0x' + i.toString(16); }
  //console.log(result);
 return result
}

function postit(url, data, callbackFunction) {

    $.ajax({
        url: url,
        type: 'POST',
        processData: false,
        contentType: false,
        datatype: 'json',
        data: data,
        success: function (response) {
            callbackFunction(response.url, response.tooltip);
        },
        error: function (){

        }
    });
}


$(document).ready(function() {
    Davinci.init(window.innerWidth, window.innerHeight);
    Davinci.animate();
    Davinci.createPainting('/images/fl.jpg', "hejsan hej");

            $('#dialog').dialog({
                modal: true,
                autoOpen: false,
                resizable: false,
                draggable: false,
                show: {
                    effect: "fade",
                    duration: 1000
                },
                hide: {
                    effect: "fade",
                    duration: 1000
                },
                width: 'auto',
                height: 'auto',
                close: function () {
                    Davinci.setEnable(); 

                }
            });

});



