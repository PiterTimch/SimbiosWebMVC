document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('createProductForm');

    form.addEventListener('submit', () => {
        savePhotos();
    });

    tinymce.init({
        selector: '#description',
        plugins: 'advlist autolink link image lists charmap preview anchor pagebreak searchreplace wordcount code fullscreen insertdatetime media table help',
        toolbar: 'undo redo | styles | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | preview fullscreen',
        menubar: 'file edit view insert format tools table help',

        images_upload_handler: async function (blobInfo, success, failure) {
            const base64 = blobInfo.base64();

            try {
                const response = await axios.post('/Admin/ImageFiles/SaveImage', JSON.stringify(base64), {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });
                return response.data;
            } catch (error) {
                console.error('Error uploading image:', error);
                failure('Помилка завантаження зображення');
            }
        },


        paste_preprocess: function (plugin, args) {
            let content = args.content.trim();
            const urlRegex = /<img[^>]*?src=["']([^"']+)["'][^>]*?>/gi;

            const matches = [...content.matchAll(urlRegex)];
            const urls = Array.from(new Set(matches.map(match => match[1]).filter(Boolean)));
            const imageRegex = /\.(jpeg|jpg|png|gif|bmp|webp|svg)(\?.*)?$/i;

            if (urls.length === 0) return;

            const editor = tinymce.activeEditor;

            if (!editor) {
                console.warn('TinyMCE editor not found.');
                return;
            }

            const uploadPromises = urls
                .filter(url => imageRegex.test(url))
                .map(url => {
                    return axios.head(url)
                        .then(response => {
                            const contentType = response.headers['content-type'];
                            if (contentType.startsWith('image/')) {
                                return axios.post('/Admin/ImageFiles/SaveImage', JSON.stringify(url), {
                                    headers: { 'Content-Type': 'application/json' }
                                }).then(res => ({ originalUrl: url, newUrl: res.data }));
                            }
                        })
                        .catch(() => null);
                });

            Promise.all(uploadPromises).then(results => {
                const updates = results.filter(Boolean);

                if (updates.length > 0) {
                    updates.forEach(({ originalUrl, newUrl }) => {
                        const images = editor.dom.select(`img[src="${originalUrl}"]`);
                        images.forEach(img => {
                            editor.dom.setAttrib(img, 'src', newUrl);
                        });
                    });
                }
            }).catch(error => {
                console.warn('Error processing images:', error);
            });
        }

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

        Array.from(files).forEach(file => {
            if (!file.type.startsWith('image/')) return;

            const reader = new FileReader();
            reader.onload = function (e) {
                const col = document.createElement('div');
                col.className = 'col';

                const wrapper = document.createElement('div');
                wrapper.className = 'position-relative';

                const img = document.createElement('img');
                img.src = e.target.result;
                img.className = 'img-fluid rounded border';
                img.style.height = '100px';
                img.style.objectFit = 'cover';
                img.alt = 'Preview';

                const removeBtn = document.createElement('button');
                removeBtn.type = 'button';
                removeBtn.className = 'btn btn-sm btn-danger position-absolute top-0 end-0 m-1 rounded-circle';
                removeBtn.innerHTML = '&times;';
                removeBtn.addEventListener('click', () => col.remove());

                wrapper.appendChild(img);
                wrapper.appendChild(removeBtn);
                col.appendChild(wrapper);
                imageList.appendChild(col);
            };
            reader.readAsDataURL(file);
        });

        fileInput.value = '';
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
