// Importações / Dependências
import { useState } from 'react';
import { supabase } from '../services/supabase'; /* Importando o nosso cliente configurado */
import { useNavigate, Link } from 'react-router-dom';


export function RegisterPage() {
  const navigate = useNavigate(); /* Instancia o controlador de tráfego */
  const [email, setEmail] = useState('');
  const [senha, setSenha] = useState('');
  const [confirmarSenha, setConfirmarSenha] = useState('');
  
  const [erro, setErro] = useState('');
  const [sucesso, setSucesso] = useState('');
  const [carregando, setCarregando] = useState(false);

  // Função para Cadastro com E-mail e Senha
  const handleRegister = async (e) => {
    e.preventDefault(); /* Esse comando bloqueia o recarregamento da página, para que não seja perdido os dados na memória do React */
    setErro(''); /* Limpa erros de tentativas anteriores */
    setSucesso('');
    setCarregando(true); /* Avisamos o React para para começar a carregar */

    // 1. Validação de campos vazios
    if (!email || !senha || !confirmarSenha) {
      setErro('Por favor, preencha todos os campos.');
      setCarregando(false);
      return;
    }

    // 2. Validação de igualdade de senhas
    if (senha !== confirmarSenha) {
      setErro('As senhas não coincidem. Tente novamente.');
      setCarregando(false);
      return;
    }

    // 3. Validação de segurança básica
    if (senha.length < 6) {
      setErro('A senha deve ter pelo menos 6 caracteres.');
      setCarregando(false);
      return;
    }

    // Chamada mágica do Supabase para CADASTRO
    const { data, error } = await supabase.auth.signUp({
      email: email,
      password: senha,
    });

    if (error) {
      setErro(error.message);
    } else {
      setSucesso('Conta criada com sucesso! Direcionando para o quiz...');

      // Aguarda 1.5 segundos apenas para o usuário ler a mensagem verde de sucesso antes de mudar a tela
      setTimeout(() => {
        navigate('/onboarding');
      }, 1500);
    }
    
    setCarregando(false);
  };

  // Função para Cadastro/Login com Google
  const handleGoogleLogin = async () => {
    setErro('');
    const { error } = await supabase.auth.signInWithOAuth({
      provider: 'google',
    });

    if (error) {
      setErro('Erro ao tentar conectar com o Google: ' + error.message);
    }
  };


  return (
    <div className="auth-container">
      <form onSubmit={handleRegister} className="auth-form">
        <h2 style={{ textAlign: 'center', margin: '0 0 10px 0', color: '#333' }}>Criar Nova Conta</h2>
        
        <div>
          {/* <label style={{ display: 'block', marginBottom: '5px', fontSize: '14px' }}>E-mail</label> */} 
          <input 
            type="email" 
            value={email}
            onChange={(e) => setEmail(e.target.value)} 
            placeholder="Digite seu e-mail"
            className="auth-input"
            disabled={carregando}
          />
        </div>

        <div>
          {/* <label style={{ display: 'block', marginBottom: '5px', fontSize: '14px' }}>Senha</label> */} 
          <input 
            type="password" 
            value={senha}
            onChange={(e) => setSenha(e.target.value)} 
            placeholder="Digite sua senha"
            className="auth-input"
            disabled={carregando}
          />
        </div>

        <div>
          {/* <label style={{ display: 'block', marginBottom: '5px', fontSize: '14px' }}>Confirmar Senha</label> */} 
          <input 
            type="password" 
            value={confirmarSenha}
            onChange={(e) => setConfirmarSenha(e.target.value)} 
            placeholder="Digite sua senha novamente"
            className="auth-input"
            disabled={carregando}
          />
        </div>

        {/* Exibição condicional de erros e sucessos */}
        {erro && <p style={{ color: 'red', fontSize: '13px', margin: 0, textAlign: 'center' }}>{erro}</p>}
        {sucesso && <p style={{ color: 'green', fontSize: '13px', margin: 0, textAlign: 'center' }}>{sucesso}</p>}

        <button type="submit" className="auth-button btn-success" disabled={carregando}>
          {carregando ? 'Criando conta...' : 'Cadastrar'}
        </button>

        <div style={{ textAlign: 'center', color: '#666', fontSize: '12px', margin: '5px 0' }}>ou</div>

        <button type="button" onClick={handleGoogleLogin} className="auth-button btn-google">
          Continuar com Google
        </button>
               
        <div style={{ textAlign: 'center', marginTop: '10px', fontSize: '13px' }}>
          Já tem uma conta? <Link to="/" style={{ color: '#007bff', textDecoration: 'none', fontWeight: 'bold' }}>Entrar</Link>
        </div>
      </form>
    </div>
  );
}
