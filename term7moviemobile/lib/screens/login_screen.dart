import 'package:term7moviemobile/utils/theme.dart';
import 'package:term7moviemobile/widgets/social-button.dart';
import 'package:flutter/material.dart';
import 'package:term7moviemobile/controllers/auth_controller.dart';
import 'package:flutter_svg/flutter_svg.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({Key? key}) : super(key: key);

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  @override
  Widget build(BuildContext context) {
    final Size _size = MediaQuery.of(context).size;

    return Scaffold(
      backgroundColor: MyTheme.backgroundColor,
      body: SizedBox(
        height: _size.height,
        width: _size.width,
        child: Padding(
          padding: const EdgeInsets.only(top: 40, left: 24, right: 24),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              SvgPicture.asset(
                "assets/images/logo.svg",
              ),
              const Padding(
                padding: EdgeInsets.only(top: 8, bottom: 24),
                child: Text(
                  'Welcome Back!',
                  style: TextStyle(
                      fontSize: 24,
                      fontWeight: FontWeight.w500,
                      color: MyTheme.textColor),
                ),
              ),
              SizedBox(
                width: double.infinity,
                child: SvgPicture.asset(
                  "assets/images/login_illustration.svg",
                  fit: BoxFit.contain,
                ),
              ),
              const SizedBox(height: 30),
              SocialButton(
                onGoogleClick: () => {AuthController.instance.googleLogin()},
              ),
            ],
          ),
        ),
      ),
    );
  }
}
