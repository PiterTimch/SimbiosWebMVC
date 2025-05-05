document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('createProductForm');

    form.addEventListener('submit', () => {
        savePhotos();
    });

    tinymce.init({
        selector: '#description',
        plugins: 'advlist autolink link image lists charmap preview anchor pagebreak searchreplace wordcount code fullscreen insertdatetime media table help',
        toolbar: 'undo redo | styles | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | preview fullscreen',
        menubar: 'file edit view insert format tools table help'
    });

    new Sortable(document.getElementById('imageList'), {
        animation: 150,
        ghostClass: 'opacity-50'
    });

    const fileInput = document.getElementById('fileInput');
    const imageList = document.getElementById('imageList');

    fileInput.addEventListener('change', handleFiles);

    function handleFiles(event) {
        const files = event.target.files;
        imageList.innerHTML = '';

        Array.from(files).forEach(file => {
            if (!file.type.startsWith('image/')) return;

            const reader = new FileReader();
            reader.onload = function (e) {
                const col = document.createElement('div');
                col.className = 'col';

                const img = document.createElement('img');
                img.src = e.target.result;
                img.className = 'img-fluid rounded border';
                img.alt = 'Preview';

                col.appendChild(img);
                imageList.appendChild(col);
            };
            reader.readAsDataURL(file);
        });
        
    }

    function savePhotos() {
        form.querySelectorAll('input[name^="Images"]').forEach(el => el.remove());

        const imgs = imageList.querySelectorAll('img');

        imgs.forEach((img, index) => {
            const hiddenName = document.createElement('input');
            hiddenName.type = 'hidden';
            hiddenName.name = `Images[${index}].Name`;
            hiddenName.value = img.src;
            form.appendChild(hiddenName);

            const hiddenPriority = document.createElement('input');
            hiddenPriority.type = 'hidden';
            hiddenPriority.name = `Images[${index}].Priority`;
            hiddenPriority.value = index;
            form.appendChild(hiddenPriority);
        });
    }
});
