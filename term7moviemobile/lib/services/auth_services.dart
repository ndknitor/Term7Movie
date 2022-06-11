import 'package:dio/dio.dart';
import 'package:term7moviemobile/utils/constants.dart';

class AuthServices {
  static Future<Response> getNewAccessToken(String? refreshToken) async {
    return await Dio().post(Constants.baseApiUrl + 'auth/token',
        data: {"refreshToken": refreshToken});
  }

  static Future<Response> postIdToken(String? idToken) async {
    return await Dio().post(Constants.baseApiUrl + 'auth/google-sign-in',
        data: {"idToken": idToken});
  }
}
