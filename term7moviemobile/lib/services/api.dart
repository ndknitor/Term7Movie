import 'package:dio/dio.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:term7moviemobile/controllers/auth_controller.dart';
import 'package:term7moviemobile/services/auth_services.dart';
import 'package:term7moviemobile/utils/constants.dart';

class Api {
  final Dio api = Dio();
  String? accessToken;

  final _storage = const FlutterSecureStorage();

  Api() {
    api.interceptors
        .add(InterceptorsWrapper(onRequest: (options, handler) async {
      if (await _storage.containsKey(key: 'accessToken')) {
        accessToken = await _storage.read(key: "accessToken");
        options.baseUrl = Constants.baseApiUrl;
        options.headers['Authorization'] = 'Bearer $accessToken';
        return handler.next(options);
      }
    }, onError: (DioError error, handler) async {
      if (error.response?.statusCode == 401) {
        if (await _storage.containsKey(key: 'refreshToken')) {
          if (await refreshToken()) {
            return handler.resolve(await _retry(error.requestOptions));
          } else {
            AuthController.instance.signOut();
          }
        }
      }
      return handler.next(error);
    }));
  }

  Future<Response<dynamic>> _retry(RequestOptions requestOptions) async {
    final options = Options(
      method: requestOptions.method,
      headers: requestOptions.headers,
    );
    return api.request<dynamic>(requestOptions.path,
        data: requestOptions.data,
        queryParameters: requestOptions.queryParameters,
        options: options);
  }

  Future<bool> refreshToken() async {
    try {
      final refreshToken = await _storage.read(key: 'refreshToken');
      final response = await AuthServices.getNewAccessToken(refreshToken);

      if (response.statusCode == 200) {
        accessToken = response.data['accessToken'];
        print(accessToken);
        await _storage.write(key: 'accessToken', value: accessToken);
        return true;
      } else {
        // refresh token is wrong
        // accessToken = null;
        // _storage.deleteAll();
        // Get.toNamed("login");
        return false;
      }
    } catch (e) {
      return false;
    }
  }
}
