
class Links extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            isSolid: this.props.isSolid
        };
    }
    render() {
        return (
            <>
                {
                    this.props.isSolid ?
                    <div class="wrapper">
                        <div class="ft-social">
                            <ul class="clearfix">
                                <li><i class="fa-brands fa-instagram"></i></li>
                                <li><i class="fa-brands fa-telegram"></i></li>
                                <li><i class="fa-brands fa-youtube"></i></li>
                            </ul>
                        </div>
                    </div>
                        :
                    <div class="wrapper white-svg">
                        <div class="ft-social">
                            <ul class="clearfix">
                                <li><i class="fa-brands fa-instagram"></i></li>
                                <li><i class="fa-brands fa-telegram"></i></li>
                                <li><i class="fa-brands fa-youtube"></i></li>
                            </ul>
                        </div>
                    </div>
                }
            </>
        );
    }
}

class MainPanel extends React.Component {
    render() {
        return (
            <>
                <div class="container-fluid">

                    <div class="row top-header gray">
                        <div class="col">
                            <div class="row">
                                <div class="col-4">
                                    <Links isSolid={true} />
                                </div>
                                <div className="text-center col-4">
                                    <h1>فروشگاه خشکبار زارع</h1>
                                </div>
                                <div class="col-4"></div>                            
                            </div>
                            <div class="row">
                                <div class="col-2"></div>
                                <div className="text-center col-8">
                                    <nav>
                                        <ul>
                                            <li class="sub-menu-parent" tab-index="2">
                                                <a href="#">ارتباط با ما</a>
                                                <ul class="sub-menu">
                                                    <li><a href="#">درباره ما</a></li>
                                                    <li><a href="#">درباره اونا</a></li>
                                                </ul></li>
                                            <li class="sub-menu-parent" tab-index="1">
                                                <a href="#">محصولات</a>
                                                <ul class="sub-menu">
                                                    <li><a href="#">خشکبار</a></li>
                                                    <li><a href="#">شکلات</a></li>
                                                    <li><a href="#">کادوئی</a></li>
                                                </ul>
                                            </li>
                                            <li class="sub-menu-parent" tab-index="0">
                                                <a href="">خانه</a>
                                                <ul class="sub-menu">
                                                    <li><a href="#">سفارشات</a></li>
                                                    <li><a href="#">خدمات</a></li>
                                                </ul>
                                            </li>
                                        </ul>
                                    </nav>
                                </div>
                                <div class="col-2"></div>                             
                            </div>                            
                        </div>
                    </div>

                    <div class="row top-header-img" >
                        <div class="col-6 light-gray h-100">
                            <img src="img/main-pic.jpg" class="logo" alt="" />
                        </div>
                        <div class="col-6 light-gray h-100">
                            <div class="right-panel">
                                <h1> خشکبار بهارشیراز</h1>
                                <h3>فروشگاه آنلاین زارع تقدیم می‌نماید</h3><br /><br />
                                <p>تمام تلاش ما جلب رضایت شما مشتریان گرامی می‌باشد<br/><br/>
                                    شما که از اقصی نقاط کشور سفارش هایتان را به ما عرضه می‌دارید</p>
                            </div>
                        </div>
                    </div>

                    <div class="row carousel-panel">
                        <div class="col-12">
                            <div id="carousel"></div>
                        </div>
                    </div>

                    <div class="row description-panel gray">   
                        <div class="col-6 light-gray h-100">
                            <div class="left-panel">
                                <h1>کادوی مهربانی</h1>
                                <h3>هدیه را برای اقوام ارزانی کن</h3><br /><br />
                                <p>چه خوش است هدیه بردن ز برای آشنایان<br/><br/>
                                    چه تشکر هم نکوتر شنوی ز میزبانان</p>
                            </div>
                        </div>                     
                        <div class="col-6 light-gray h-100">
                            <img src="img/Chocolate3.jpg" class="logo" alt="" />
                        </div>
                    </div>

                    <div class="row below-panel">
                        <div class="col-3">
                            
                        </div>

                        <div class="col-2">
                            <img src="img/Chocolate3.jpg" class="order-img" alt="" />
                            <div className="text-center pt-5">
                                <h4>شکلات کازرون</h4>
                                <p>بسته بندی شکیل</p>
                                <button type="button" class="btn btn-secondary">سفارش</button>
                            </div>
                        </div>
                        <div class="col-2">
                            <img src="img/Nuts3.jpg" class="order-img" alt="" />
                            <div className="text-center pt-5">
                                <h4>آجیل بیضا</h4>
                                <p>درجه یک</p>
                                <button type="button" class="btn btn-secondary">سفارش</button>
                            </div>
                        </div>
                        <div class="col-2">
                            <img src="img/Chocolate1.jpg" class="order-img" alt="" />
                            <div className="text-center pt-5">
                                <h4>کاکائوی سعدی</h4>
                                <p>مخصوص محمد زارع</p>
                                <button type="button" class="btn btn-secondary">سفارش</button>
                            </div>
                        </div>

                        <div class="col-3">
                            
                        </div>
                    </div>

                    <div class="waveContainer row h-25">
                        <svg viewBox="0 0 500 150" preserveAspectRatio="none" class="viewBox">
                            <path d="M0.00,49.98 C254.51,72.06 306.43,22.41 500.00,49.98 L500.00,150.00 L0.00,150.00 Z" class="path">
                            </path>
                        </svg>
                    </div>

                    <div class="row footer-panel dark-gray">
                        <div class="col-12">
                            <Links isSolid={false} />
                        </div>
                    </div>
                </div>            
            </>
        );
    }
}

ReactDOM.render(<MainPanel />, document.getElementById('content'));