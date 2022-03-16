
class ResetPassPanel extends React.Component {
    render() {
        return (
            <>
                <div className="resetpass-panel">
                    <div id="bg"></div>
                    <div className="form">
                        <h5 className="title">تغییر پسورد</h5>


                        <div class="form-field">
                            <input type="password" placeholder="رمز عبور" required />
                        </div>

                        <div class="form-field">
                            <input type="password" placeholder="تکرار رمز عبور" required />
                        </div>

                        <div class="form-field">
                            <button class="btn" type="submit">تغییر</button>
                        </div>
                    </div>
                </div>
            </>
        );
    }
}

ReactDOM.render(<ResetPassPanel />, document.getElementById('content'));