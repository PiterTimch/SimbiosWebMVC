document.addEventListener('DOMContentLoaded', () => {
    // Ініціалізація TinyMCE
    tinymce.init({
        selector: '#description',
        plugins: 'advlist autolink link image lists charmap preview anchor pagebreak searchreplace wordcount code fullscreen insertdatetime media table help',
        toolbar: 'undo redo | styles | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | preview fullscreen',
        menubar: 'file edit view insert format tools table help'
    });

    // Ініціалізація Sortable
    new Sortable(document.getElementById('imageList'), {
        animation: 150,
        ghostClass: 'opacity-50'
    });

    // Зображення preview
    const fileInput = document.getElementById('fileInput');
    const imageList = document.getElementById('imageList');

    fileInput.addEventListener('change', handleFiles);

    function handleFiles(event) {
        const files = event.target.files;
        imageList.innerHTML = ''; // очистка попередніх preview

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
});
