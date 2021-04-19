let $data = new FormData();
$data.append("sort", "date");
$data.append("layout", "grid");
let nextHandler = async function (pageIndex) {
    $data.set('page', pageIndex);
    let data = await fetch('./List', {method: 'POST', body: $data});
    $(".cars").append(await data.text());
    return data.status === 200;

};
let scroller = new InfiniteAjaxScroll('.cars', {
    item: '.item',
    next: nextHandler,
    spinner: '.spinner',
    delay: 600
})

function ShowList(idContent, idImage) {
    let content = document.querySelector(idContent);
    content.classList.toggle("d-none");
    let img = document.querySelector(idImage);
    if (img.getAttribute("d").startsWith("M4.646 1")) {
        img.setAttribute("d", "M1.646 4.646a.5.5 0 0 1 .708 0L8 10.293l5.646-5.647a.5.5 0 0 1 .708.708l-6 6a.5.5 0 0 1-.708 0l-6-6a.5.5 0 0 1 0-.708z")
    } else {
        img.setAttribute("d", "M4.646 1.646a.5.5 0 0 1 .708 0l6 6a.5.5 0 0 1 0 .708l-6 6a.5.5 0 0 1-.708-.708L10.293 8 4.646 2.354a.5.5 0 0 1 0-.708z")
    }
    return false;
}

document.querySelector("#dataForm").addEventListener("change", Select);

async function Select() {
    scroller.pause();
    let $sort = $data.get("sort"), $layout = $data.get("layout");
    $data = new FormData(document.querySelector("#dataForm"));
    $data.append("sort", $sort);
    $data.append("layout", $layout);
    let response = await fetch('./List', {method: 'POST', body: $data});
    document.querySelector("div.cars").innerHTML = await response.text();
    if (response.status === 200) {
        scroller.pageIndex = 0;
        scroller.resume();
    }
}

let sort = document.querySelectorAll('.sort');
let layout = document.querySelectorAll('.layout');
[].forEach.call(sort, function (el) {
    el.onclick = function () {
        sort.forEach(element => {
            element.classList.remove("selected");
        })
        $data.set("sort", this.getAttribute("value"));
        this.classList.add("selected");
        Select().then(() => {
            return false
        });
    }
});
[].forEach.call(layout, function (el) {
    el.onclick = function () {
        layout.forEach(element => {
            element.classList.remove("selected");
        })
        let value = this.getAttribute("value");
        $data.set("layout", value);
        this.classList.add("selected");
        Select().then(() => {
            if (value === "list") document.querySelector(".cars").classList.add("mt-1");
            else document.querySelector(".cars").classList.remove("mt-1");
            return false
        });
    }
});

function clearFilter() {
    sort.forEach(element => {
        element.classList.remove("selected")
    })
    $data.set("sort", "date")
    $data.set("layout", "grid")
    layout.forEach(element => {
        element.classList.remove("selected");
    })
    sort.forEach(element => {
        element.classList.remove("selected");
    })
    sort[0].classList.add("selected")
    layout[0].classList.add("selected")
    let form = document.querySelector("#dataForm")
    form.reset()
    form.dispatchEvent(new Event("change"))
}
