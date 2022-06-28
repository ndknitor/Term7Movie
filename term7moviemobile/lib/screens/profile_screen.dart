import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:term7moviemobile/controllers/auth_controller.dart';
import 'package:term7moviemobile/controllers/profile_controller.dart';
import 'package:term7moviemobile/utils/constants.dart';
import 'package:term7moviemobile/utils/theme.dart';

class ProfileScreen extends StatefulWidget {
  const ProfileScreen({Key? key}) : super(key: key);

  @override
  State<ProfileScreen> createState() => _ProfileScreenState();
}

class _ProfileScreenState extends State<ProfileScreen> with SingleTickerProviderStateMixin {
  final nameController = TextEditingController();
  final emailController = TextEditingController();
  late AnimationController _animationController;
  late Animation<Offset> _animationImage, _animationCamera;

  @override
  void initState() {
    super.initState();
    Get.put(ProfileController());
    _animationController = AnimationController(
        vsync: this, duration: const Duration(milliseconds: 300));
    _animationImage =
        Tween<Offset>(begin: Offset.zero, end: const Offset(0.0, -0.4))
            .animate(_animationController);
    _animationCamera =
        Tween<Offset>(begin: Offset.zero, end: const Offset(0.0, -1.1))
            .animate(_animationController);
  }

  @override
  Widget build(BuildContext context) {
    String? name = AuthController.instance.user!.displayName ?? "No Name";
    String? email = AuthController.instance.user!.email;
    nameController.text = name;
    emailController.text = email.toString();
    return Scaffold(
      body: SafeArea(
        child: Container(
          height: double.infinity,
          width: double.infinity,
          child: Stack(
            children: [
              NotificationListener<ScrollNotification>(
                onNotification: (notification) {
                  if (notification.metrics.pixels == notification.metrics.maxScrollExtent) {
                    _animationController.forward();
                  } else if (notification.metrics.pixels == notification.metrics.minScrollExtent * 0.5) {
                    _animationController.reverse();
                  }
                  print(notification.metrics.pixels);
                  return true;
                },
                child: SingleChildScrollView(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Container(
                        height: 150,
                        width: double.maxFinite,
                        decoration: const BoxDecoration(
                          color: MyTheme.backgroundColor,
                          // borderRadius: BorderRadius.only(
                          //   bottomRight: Radius.circular(40),
                          //   bottomLeft: Radius.circular(40),
                          // ),
                        ),
                      ),
                      const SizedBox(
                        height: 60,
                      ),
                      const Padding(
                        padding: EdgeInsets.only(left: 70.0),
                        child: Text("Name", style: TextStyle(fontWeight: FontWeight.w600)),
                      ),
                      Obx(
                            () {
                          return Padding(
                            padding: const EdgeInsets.symmetric(horizontal: 20.0),
                            child: TextFormField(
                              readOnly: ProfileController.instance.isEdit.value,
                              initialValue: name,
                              style: const TextStyle(color: Colors.black),
                              decoration: InputDecoration(
                                border: const OutlineInputBorder(borderSide: BorderSide.none),
                                prefixIcon: Padding(
                                  padding: const EdgeInsets.all(4.0),
                                  child: Container(
                                    height: 20,
                                    width: 20,
                                    decoration: BoxDecoration(
                                      borderRadius: BorderRadius.circular(20),
                                      color: const Color(0xFFE4EDFF),
                                    ),
                                    child: const Icon(
                                      Icons.person_outline,
                                      color: Color(0xFF4C7EFF),
                                    ),
                                  ),
                                ),
                                suffixIcon: GestureDetector(
                                  onTap: () {
                                    ProfileController.instance.toggleEdit();
                                  },
                                  child: const Padding(
                                    padding: EdgeInsets.all(4.0),
                                    child: Icon(
                                      Icons.edit_outlined,
                                      color: Color(0xFF4C7EFF),
                                    ),
                                  ),
                                ),
                              ),
                            ),
                          );
                        },
                      ),
                      const SizedBox(
                        height: 10,
                      ),
                      const Padding(
                        padding: EdgeInsets.only(left: 70.0),
                        child: Text("Email", style: TextStyle(fontWeight: FontWeight.w600)),
                      ),
                      Padding(
                        padding: const EdgeInsets.symmetric(horizontal: 20.0),
                        child: TextFormField(
                          initialValue: email,
                          readOnly: true,
                          style: TextStyle(color: Colors.black),
                          decoration: InputDecoration(
                            border: const OutlineInputBorder(borderSide: BorderSide.none),
                            prefixIcon: Padding(
                              padding: const EdgeInsets.all(4.0),
                              child: Container(
                                height: 20,
                                width: 20,
                                decoration: BoxDecoration(
                                  borderRadius: BorderRadius.circular(20),
                                  color: const Color(0xFFE8DBFF),
                                ),
                                child: const Icon(
                                  Icons.email_outlined,
                                  color: Color(0xFFCC80EA),
                                ),
                              ),
                            ),
                          ),
                        ),
                      ),
                      const SizedBox(
                        height: 10,
                      ),
                      const Divider(
                        thickness: 1,
                        color: MyTheme.borderColor,
                      ),
                      const Padding(
                        padding: EdgeInsets.only(left: 30),
                        child: Text('OTHERS'),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(left: 20),
                        child: ListTile(
                          leading: const Icon(
                            Icons.history_outlined,
                            size: 20,
                          ),
                          title: const Text(
                            'Transaction',
                            style: TextStyle(color: Colors.black),
                          ),
                          onTap: () {},
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(left: 20),
                        child: ListTile(
                          leading: const Icon(
                            Icons.logout_outlined,
                            size: 20,
                            color: Colors.red,
                          ),
                          title: const Text(
                            'Logout',
                            style: TextStyle(color: Colors.red),
                          ),
                          onTap: () {
                            AuthController.instance.signOut();
                          },
                        ),
                      ),
                      SizedBox(
                        height: 20,
                      )
                    ],
                  ),
                ),
              ),
              Positioned(
                top: 90,
                left: MediaQuery.of(context).size.width * 0.5 - 60,
                child: SlideTransition(
                  position: _animationImage,
                  child: Center(
                    child: Container(
                      decoration: BoxDecoration(
                        border: Border.all(color: MyTheme.borderColor, width: 3),
                        borderRadius: BorderRadius.circular(60),
                      ),
                      child: ClipRRect(
                        borderRadius: BorderRadius.circular(60),
                        child: CachedNetworkImage(
                          fit: BoxFit.cover,
                          imageUrl: AuthController.instance.user!.photoURL ??
                              Constants.defaultAvatar,
                          height: 100,
                          width: 100,
                        ),
                      ),
                    ),
                  ),
                ),
              ),
              Positioned(
                top: 155,
                left: MediaQuery.of(context).size.width * 0.5 + 10,
                child: SlideTransition(
                  position: _animationCamera,
                  child: Container(
                    height: 40,
                    width: 40,
                    decoration: BoxDecoration(
                      borderRadius: BorderRadius.circular(25),
                      color: MyTheme.primaryColor,
                    ),
                    child: const Icon(
                      Icons.camera_alt,
                      color: Colors.white,
                    ),
                  ),
                ),
              )
            ],
          ),
        ),
      ),
    );
  }
}
