import 'dart:async';
import 'dart:convert';
import 'package:dio/dio.dart';
import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:get/get.dart';
import 'package:google_sign_in/google_sign_in.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:term7moviemobile/services/api.dart';
import 'package:term7moviemobile/services/auth_services.dart';
import 'package:term7moviemobile/services/room_services.dart';
import 'package:term7moviemobile/utils/constants.dart';
import 'package:term7moviemobile/utils/theme.dart';

class AuthController extends GetxController {
  static AuthController instance = Get.find();
  final storage = new FlutterSecureStorage();
  final GoogleSignIn googleSignIn = GoogleSignIn();
  late Rx<User?> _user;
  bool isLoging = false;
  User? get user => _user.value;
  final FirebaseAuth auth = FirebaseAuth.instance;
  int? isviewed;

  @override
  void onReady() {
    super.onReady();
    _user = Rx<User?>(auth.currentUser);
    _user.bindStream(auth.authStateChanges());
    ever(_user, loginRedirect);
  }

  loginRedirect(User? user) {
    Timer(Duration(seconds: isLoging ? 0 : 2), () async {
      SharedPreferences prefs = await SharedPreferences.getInstance();
      isviewed = prefs.getInt('onBoard');
      print(isviewed);
      if (user == null) {
        isLoging = false;
        update();
        isviewed != 0 ? Get.offAllNamed('/onboard') : Get.offAllNamed('/login');
      } else {
        isLoging = true;
        update();
        Get.offAllNamed('/');
      }
    });
  }

  void googleLogin() async {
    isLoging = true;
    update();
    try {
      final GoogleSignInAccount? googleSignInAccount =
          await googleSignIn.signIn();
      if (googleSignInAccount != null) {
        final GoogleSignInAuthentication? googleAuth =
            await googleSignInAccount.authentication;
        final crendentials = GoogleAuthProvider.credential(
          accessToken: googleAuth?.accessToken,
          idToken: googleAuth?.idToken,
        );
        await auth.signInWithCredential(crendentials);
        var token = await auth.currentUser?.getIdToken();
        final res = await AuthServices.postIdToken(token);

        if (res.statusCode == 200) {
          await storage.write(
              key: 'accessToken', value: res.data['accessToken']);
          await storage.write(
              key: 'refreshToken', value: res.data['refreshToken']);
        }
        getSuccessSnackBar("Successfully logged in as ${_user.value!.email}");
        //RoomServices.getRoomById("1").then((value) => print(value));
      }
    } on FirebaseAuthException catch (e) {
      getErrorSnackBar("Google Login Failed", e);
    } on PlatformException catch (e) {
      getErrorSnackBar("Google Login Failed", e);
    }
  }

  getErrorSnackBar(String message, _) {
    Get.snackbar(
      "Error",
      "$message\n${_.message}",
      snackPosition: SnackPosition.BOTTOM,
      backgroundColor: MyTheme.errorColor.withOpacity(0.25),
      colorText: MyTheme.errorColor,
      borderRadius: 10,
      margin: const EdgeInsets.only(bottom: 10, left: 10, right: 10),
    );
  }

  getSuccessSnackBar(String message) {
    Get.snackbar(
      "Success",
      message,
      snackPosition: SnackPosition.BOTTOM,
      backgroundColor: MyTheme.successColor.withOpacity(0.25),
      colorText: MyTheme.successColor,
      borderRadius: 10,
      margin: const EdgeInsets.only(bottom: 10, left: 10, right: 10),
    );
  }

  void signOut() async {
    storage.deleteAll();
    await auth.signOut();
  }
}
