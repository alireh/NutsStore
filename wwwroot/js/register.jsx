
class RegisterPanel extends React.Component {
    render() {
        return (
            <>
                <div className="register-panel">
                    <div id="bg"></div>
                    <div className="form">
                        <h5 className="title">ورود / ثبت نام</h5>
                        <div class="form-field">
                            <input type="text" placeholder="نام کاربری" required />
                        </div>

                        <div class="form-field">
                            <input type="text" placeholder="نام" required />
                        </div>

                        <div class="form-field">
                            <input type="text" placeholder="نام خانوادگی" required />
                        </div>

                        <div class="form-field">
                            <input type="email" placeholder="ایمیل" required />
                        </div>

                        <div class="form-field">
                            <input type="text" placeholder="نشانی" required />
                        </div>

                        <div class="form-field">
                            <section>
                                <div className="gender-label">جنسیت</div>
                                <select className="select-gender" onchange="changeColor(this)">
                                    <option value="#ff2323">مرد</option>
                                    <option value="#9b1fe8">زن</option>
                                </select>
                            </section>
                        </div>

                        <div class="form-field">
                            <input type="text" placeholder="شماره تلفن" required />
                        </div>

                        <div class="form-field">
                            <input type="password" placeholder="رمز عبور" required />
                        </div>

                        <div class="form-field">
                            <input type="password" placeholder="تکرار رمز عبور" required />
                        </div>

                        <div class="form-field">
                            <button class="btn" type="submit">ثبت نام</button>
                        </div>
                    </div>
                </div>
            </>
        );
    }
}

ReactDOM.render(<RegisterPanel />, document.getElementById('content'));