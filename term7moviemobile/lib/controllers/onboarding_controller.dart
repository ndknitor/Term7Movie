import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:term7moviemobile/models/onboarding_model.dart';

class OnBoardingController extends GetxController {
  RxInt currentPage = 0.obs;
  bool get isLastPage => currentPage.value == data.length - 1;
  PageController pageController = PageController();

  forwardAction() {
    if (isLastPage) {
      Get.toNamed("/login");
    } else
      pageController.nextPage(duration: 200.milliseconds, curve: Curves.ease);
  }

  skipAction() {
      Get.toNamed("/login");
  }

  storeOnboardInfo() async {
    print("Shared pref called");
    int isViewed = 0;
    SharedPreferences prefs = await SharedPreferences.getInstance();
    await prefs.setInt('onBoard', isViewed);
    print(prefs.getInt('onBoard'));
  }

  final List<OnBoardingModel> data = [
    OnBoardingModel("assets/images/splash1.png", "We help you find the best movie tickets at great deals"),
    OnBoardingModel("assets/images/splash2.png", "Easy online payment"),
    OnBoardingModel("assets/images/splash3.png", "Enjoy!!!"),
  ];
}