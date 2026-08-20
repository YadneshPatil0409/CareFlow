package com.example.demo.service;

import com.example.demo.dto.UserRegisterRequest;
import com.example.demo.dto.UserResponse;
import java.util.List;

public interface UserService {

	UserResponse registerUser(UserRegisterRequest request);
    UserResponse getUserById(Long id);
    List<UserResponse> getAllUsers();
    
    
}
