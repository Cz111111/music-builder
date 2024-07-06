package com.example.demo.model;

import com.alibaba.fastjson.annotation.JSONField;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import lombok.Data;

@Entity
@Data
public class User {
    @Id
    private String username;
    @JSONField(serialize = false)
    private String password;
    private String email;
    private String nickname;

}
