/************************************
1. If you want to add or remove items you will need to change a variable called $slide-count in the CSS *minimum 3 slides

2. if you want to change the dimensions of the slides you will need to edit the slideWidth variable here 👇 and the $slide-width variable in the CSS.
************************************/
const slideWidth = 30;

const _items = [
    {
        player: {
            title: 'شکلات کاکائویی',
            desc: 'بخور حالشو ببر خیلی خوشمزه است.',
            image: 'img/Chocolate1.jpg'
        }
    },


    {
        player: {
            title: "کادوی شکلاتی",
            desc: "انصافا یه بسته به مهمونا هدیه بدی یه عمر نوکرت میشن",
            image: 'img/Kado2.jpg'
        }
    },


    {
        player: {
            title: 'آجیل مشتی',
            desc: 'اگه این آجیل رو بخوری دیگه به چیزی غیر این نمیگی آجیل.',
            image: 'img/Nuts5.jpg'
        }
    },


    {
        player: {
            title: 'بادوم فرد اعلا',
            desc: 'هر کی از این بادوم خورده دیگه بهشت نمیخاد بره چون این خود بهشته',
            image: 'img/Almond2.jpg'
        }
    },


    {
        player: {
            title: 'شکلات دیوانه کننده',
            desc: 'این شکلات از خود محمد زارع هم بهتره خودت ببین دیگه چیه؟!!!!!',
            image: 'img/Chocolate1.jpg'
        }
    }];




const length = _items.length;
_items.push(..._items);

const sleep = (ms = 0) => {
    return new Promise(resolve => setTimeout(resolve, ms));
};

const createItem = (position, idx) => {
    const item = {
        styles: {
            transform: `translateX(${position * slideWidth}rem)`
        },

        player: _items[idx].player
    };


    switch (position) {
        case length - 1:
        case length + 1:
            item.styles = { ...item.styles, filter: 'grayscale(1)' };
            break;
        case length:
            break;
        default:
            item.styles = { ...item.styles, opacity: 0 };
            break;
    }


    return item;
};

const CarouselSlideItem = ({ pos, idx, activeIdx }) => {
    const item = createItem(pos, idx, activeIdx);

    return /*#__PURE__*/(
        React.createElement("li", { className: "carousel__slide-item", style: item.styles }, /*#__PURE__*/
            React.createElement("div", { className: "carousel__slide-item-img-link" }, /*#__PURE__*/
                React.createElement("img", { src: item.player.image, alt: item.player.title })), /*#__PURE__*/

            React.createElement("div", { className: "carousel-slide-item__body" }, /*#__PURE__*/
                React.createElement("h4", null, item.player.title), /*#__PURE__*/
                React.createElement("p", null, item.player.desc))));



};

const keys = Array.from(Array(_items.length).keys());

const Carousel = () => {
    const [items, setItems] = React.useState(keys);
    const [isTicking, setIsTicking] = React.useState(false);
    const [activeIdx, setActiveIdx] = React.useState(0);
    const bigLength = items.length;

    const prevClick = (jump = 1) => {
        if (!isTicking) {
            setIsTicking(true);
            setItems(prev => {
                return prev.map((_, i) => prev[(i + jump) % bigLength]);
            });
        }
    };

    const nextClick = (jump = 1) => {
        if (!isTicking) {
            setIsTicking(true);
            setItems(prev => {
                return prev.map(
                    (_, i) => prev[(i - jump + bigLength) % bigLength]);

            });
        }
    };

    const handleDotClick = idx => {
        if (idx < activeIdx) prevClick(activeIdx - idx);
        if (idx > activeIdx) nextClick(idx - activeIdx);
    };

    React.useEffect(() => {
        if (isTicking) sleep(300).then(() => setIsTicking(false));
    }, [isTicking]);

    React.useEffect(() => {
        setActiveIdx((length - items[0] % length) % length); // prettier-ignore
    }, [items]);

    return /*#__PURE__*/(
        React.createElement("div", { className: "carousel__wrap" }, /*#__PURE__*/
            React.createElement("div", { className: "carousel__inner" }, /*#__PURE__*/
                React.createElement("button", { className: "carousel__btn carousel__btn--prev", onClick: () => prevClick() }, /*#__PURE__*/
                    React.createElement("i", { className: "fa-solid fa-angle-left carousel-arrow" })), /*#__PURE__*/

                React.createElement("div", { className: "carousel__container" }, /*#__PURE__*/
                    React.createElement("ul", { className: "carousel__slide-list" },
                        items.map((pos, i) => /*#__PURE__*/
                            React.createElement(CarouselSlideItem, {
                                key: i,
                                idx: i,
                                pos: pos,
                                activeIdx: activeIdx
                            })))), /*#__PURE__*/




                React.createElement("button", { className: "carousel__btn carousel__btn--next", onClick: () => nextClick() }, /*#__PURE__*/
                    React.createElement("i", { className: "fa-solid fa-angle-right carousel-arrow" })), /*#__PURE__*/

                React.createElement("div", { className: "carousel__dots" },
                    items.slice(0, length).map((pos, i) => /*#__PURE__*/
                        React.createElement("button", {
                            key: i,
                            onClick: () => handleDotClick(i),
                            className: i === activeIdx ? 'dot active' : 'dot'
                        }))))));






};

ReactDOM.render( /*#__PURE__*/React.createElement(Carousel, null), document.getElementById('carousel'));