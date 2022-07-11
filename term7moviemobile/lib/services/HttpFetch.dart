import 'package:http/http.dart' as http;

class HttpFetch {
  static String? host;
  static Map<String, String> headers = new Map<String, String>();
  static Future<http.Response> get(String url) async {
    if (headers == null) {
      headers = Map();
    }
    http.Response response =
        await http.get(Uri.parse(host! + url), headers: headers);

    if (response.headers['set-cookie'] != null) {
      headers["cookie"] = response.headers['set-cookie']!;
    }
    return response;
  }

  static Future<http.Response> post(String url, Object body,
      {bool multipart = false}) async {
    if (headers == null) {
      headers = Map();
    }
    if (body is String) {
      headers['Content-Type'] = "application/json";
    } else if (multipart) {
      headers['Content-Type'] = "multipart/form-data";
    } else {
      headers['Content-Type'] = "application/x-www-form-urlencoded";
    }
    http.Response response =
        await http.post(Uri.parse(host! + url), body: body, headers: headers);
    if (response.headers['set-cookie'] != null) {
      headers["cookie"] = response.headers['set-cookie']!;
    }
    return response;
  }

  static Future<http.Response> put(String url, Object body,
      {bool multipart = false}) async {
    if (headers == null) {
      headers = Map();
    }
    if (body is String) {
      headers['Content-Type'] = "application/json";
    } else if (multipart) {
      headers['Content-Type'] = "multipart/form-data";
    } else {
      headers['Content-Type'] = "application/x-www-form-urlencoded";
    }
    http.Response response =
        await http.put(Uri.parse(host! + url), body: body, headers: headers);

    if (response.headers['set-cookie'] != null) {
      headers["cookie"] = response.headers['set-cookie']!;
    }
    return response;
  }

  static Future<http.Response> delete(String url) async {
    if (headers == null) {
      headers = Map();
    }
    http.Response response =
        await http.delete(Uri.parse(host! + url), headers: headers);

    if (response.headers['set-cookie'] != null) {
      headers["cookie"] = response.headers['set-cookie']!;
    }
    return response;
  }
}
