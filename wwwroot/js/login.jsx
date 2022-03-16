class LoginPanel extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            username: '',
            password: ''
        };
    }
    login = () => {
        debugger

        var url = '/api/authentication/signin';
        var input = {
            username: this.state.username,
            password: this.state.password,
        };

        axios
            .post(url, input)
            .then(result => {
                var response = result.data;
                if (response.status == 200) {
                    var root = location.protocol + '//' + location.host;
                    window.location.href = root;
                }
            })
            .catch(err => {

            })
    }
    onUsernameChange(value) {
        this.setState({
            username: value,
        });
    }
    onPassChange(value) {
        this.setState({
            password: value,
        });
    }
    render() {
        return (
            <>
                <div className="login-panel">
                    <div id="bg"></div>
                    <div className="form">
                        <h5 className="title">ورود / ثبت نام</h5>
                        <div className="form-field">
                            <input type="text"
                                   placeholder="نام کاربری"
                                   value={this.state.username}
                                   onChange={e => this.onUsernameChange(e.target.value)}
                                   required />
                        </div>

                        <div className="form-field">
                            <input type="password"
                                   placeholder="رمز عبور"
                                   value={this.state.password}
                                   onChange={e => this.onPassChange(e.target.value)}
                                   required />
                        </div>

                        <div className="form-field">
                            <button className="btn"
                                    onClick={this.login}
                                    >ورود</button>
                        </div>
                    </div>
                </div>
            </>
        );
    }
}

ReactDOM.render(<LoginPanel />, document.getElementById('content'));