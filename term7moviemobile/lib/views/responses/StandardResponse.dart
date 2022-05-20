import 'dart:convert';
class StandardResponse {
    StandardResponse({
        this.message,
    });

    String message;

    factory StandardResponse.fromRawJson(String str) => StandardResponse.fromJson(json.decode(str));

    String toRawJson() => json.encode(toJson());

    factory StandardResponse.fromJson(Map<String, dynamic> json) => StandardResponse(
        message: json["message"],
    );

    Map<String, dynamic> toJson() => {
        "message": message,
    };
}
