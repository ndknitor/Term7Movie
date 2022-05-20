import 'dart:convert';

class SignInRequest {
    SignInRequest({
        this.email,
        this.password,
    });

    String email;
    String password;

    factory SignInRequest.fromRawJson(String str) => SignInRequest.fromJson(json.decode(str));

    String toRawJson() => json.encode(toJson());

    factory SignInRequest.fromJson(Map<String, dynamic> json) => SignInRequest(
        email: json["email"],
        password: json["password"],
    );

    Map<String, dynamic> toJson() => {
        "email": email,
        "password": password,
    };
}
