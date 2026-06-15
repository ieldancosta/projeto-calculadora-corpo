// Importações / Dependências
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'; 
import { supabase } from '../services/supabase'; /* Importando o nosso cliente configurado */


export function QuizPage() {
  const navigate = useNavigate();
  const [passoAtual, setPassoAtual] = useState(1);
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState('');

  // Um único estado para guardar todo o formulário (muito mais limpo do que criar 8 useStates)
  // 1. Adicionado o 'username' no estado inicial
  const [formData, setFormData] = useState({
    nome: '',
    username: '', // Novo campo para o @
    idade: '',
    sexo: '',
    peso: '',
    altura: '',
    percentualGordura: '',
    fatorAtividade: '1.2', // Valor padrão (Sedentário) | Defesa em profundidade
    objetivoFisico: 'Manutencao' // Valor padrão que casa com a sua Enum no C# | Defesa em profundidade
  });

  // Função para atualizar os dados do formulário conforme o usuário digita
  const handleChange = (e) => {
    const { name, value } = e.target;
    
    // Pequeno truque de UX: Se ele estiver digitando no campo username, 
    // já removemos os espaços e deixamos minúsculo em tempo real
    const valorTratado = name === 'username' ? value.toLowerCase().replace(/\s+/g, '') : value;

    setFormData((prev) => ({
      ...prev,
      [name]: valorTratado
    }));
  };

  // Funções de navegação entre os passos
  const avancarPasso = () => {
    setErro('');
    // Validação simples antes de avançar
    // 2. Validação atualizada para exigir o username
    if (passoAtual === 1 && (!formData.nome || !formData.username || !formData.idade || !formData.sexo)) {
      setErro('Por favor, preencha todos os campos obrigatórios deste passo.');
      return;
    }
    if (passoAtual === 2 && (!formData.peso || !formData.altura)) {
      setErro('O peso e a altura são obrigatórios para os cálculos metabólicos.');
      return;
    }
    setPassoAtual((prev) => prev + 1);
  };

  const voltarPasso = () => {
    setErro('');
    setPassoAtual((prev) => prev - 1);
  };

  // Envio final para o banco de dados
  const handleSubmit = async (e) => {
    e.preventDefault();
    setErro('');
    setCarregando(true);

    try {
      // 1. Pegar qual é o ID do usuário que está logado no momento
      const { data: { user } } = await supabase.auth.getUser();

      if (!user) {
        setErro('Erro: Nenhum usuário logado encontrado. Faça login novamente.');
        setCarregando(false);
        return;
      }

      // 2. Preparar o pacote de dados convertendo os textos para números onde o C# exige
      // 3. Empacotando o username junto com os dados numéricos
      const perfilParaSalvar = {
        id: user.id, // A Chave Estrangeira que liga com a tabela auth.users
        nome: formData.nome,
        username: formData.username, // Já está minúsculo e sem espaços graças ao handleChange
        idade: parseInt(formData.idade),
        sexo: formData.sexo,
        peso: parseFloat(formData.peso.replace(',', '.')), // Prevenção caso o usuário digite vírgula
        altura: parseFloat(formData.altura),
        percentual_gordura: formData.percentualGordura ? parseFloat(formData.percentualGordura.replace(',', '.')) : null,
        fator_atividade: parseFloat(formData.fatorAtividade),
        objetivo_fisico: formData.objetivoFisico
      };

      // 3. Salvar no Supabase (Você precisará criar a tabela 'perfis' lá no painel depois)
      const { error } = await supabase
        .from('perfis')
        .insert([perfilParaSalvar]);

      if (error) throw error;

      // 4. Sucesso! Redireciona para o fórum/painel principal
      alert('Perfil configurado com sucesso! Bem-vindo.');
      navigate('/feed'); // Redirecionamento suave do React Router Dom

    } catch (err) {
      // 4. A Mágica de capturar a violação de "UNIQUE" do PostgreSQL
      if (err.code === '23505') { 
        setErro(`O usuário @${formData.username} já está em uso. Por favor, escolha outro.`);
        setPassoAtual(1); // Volta o usuário para o passo 1 para ele consertar o nome
      } else {
        setErro(err.message);
      }
    } finally {
      setCarregando(false);
    }
  };

  // --- Estilos ---
  const containerStyle = {
    display: 'flex', flexDirection: 'column', alignItems: 'center',
    justifyContent: 'center', minHeight: '100vh', fontFamily: 'Arial, sans-serif',
    backgroundColor: '#f4f4f9', padding: '20px'
  };

  const formStyle = {
    backgroundColor: '#fff', padding: '30px', borderRadius: '8px',
    boxShadow: '0 4px 6px rgba(0,0,0,0.1)', width: '100%', maxWidth: '400px',
    display: 'flex', flexDirection: 'column', gap: '15px'
  };

  const inputStyle = {
    width: '100%', padding: '10px', borderRadius: '4px',
    border: '1px solid #ccc', boxSizing: 'border-box', marginTop: '5px'
  };

  const buttonRowStyle = {
    display: 'flex', justifyContent: 'space-between', marginTop: '20px', gap: '10px'
  };

  const buttonStyle = {
    flex: 1, padding: '12px', color: 'white', border: 'none', borderRadius: '4px',
    cursor: carregando ? 'not-allowed' : 'pointer', fontWeight: 'bold'
  };

  return (
    <div style={containerStyle}>
      <form style={formStyle}>
        
        {/* Cabeçalho do Wizard */}
        <div style={{ textAlign: 'center', marginBottom: '20px' }}>
          <h2 style={{ margin: '0 0 5px 0', color: '#333' }}>Configurar Perfil</h2>
          <p style={{ margin: 0, color: '#666', fontSize: '14px' }}>Passo {passoAtual} de 3</p>
          
          {/* Barra de progresso simples */}
          <div style={{ width: '100%', height: '6px', backgroundColor: '#e2e8f0', borderRadius: '3px', marginTop: '10px' }}>
            <div style={{ width: `${(passoAtual / 3) * 100}%`, height: '100%', backgroundColor: '#007bff', borderRadius: '3px', transition: 'width 0.3s ease' }}></div>
          </div>
        </div>

        {/* --- PASSO 1: DADOS PESSOAIS --- */}
        {passoAtual === 1 && (
          <div>
            <h3 style={{ fontSize: '16px', color: '#059669', borderBottom: '1px solid #eee', paddingBottom: '5px' }}>Quem é você?</h3>
            
            <div style={{ marginBottom: '15px' }}>
              <label style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}>Nome de Exibição *</label>
              <input type="text" name="nome" value={formData.nome} onChange={handleChange} placeholder="Como quer ser chamado?" style={inputStyle} />
            </div>

            <div style={{ marginBottom: '15px' }}>
              <label style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}>Nome de Usuário (@) *</label>
              <input type="text" name="username" value={formData.username} onChange={handleChange} placeholder="ex: dev_gamic" style={inputStyle} />
              <p style={{ margin: '5px 0 0 0', fontSize: '11px', color: '#64748b' }}>Este será o seu identificador único na rede social.</p>
            </div>
            
            <div style={{ display: 'flex', gap: '15px', marginBottom: '15px' }}>
              <div style={{ flex: 1 }}>
                <label style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}>Idade *</label>
                <input type="number" name="idade" value={formData.idade} onChange={handleChange} placeholder="Ex: 25" style={inputStyle} min="0" />
              </div>
              <div style={{ flex: 1 }}>
                <label style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}>Sexo *</label>
                <select name="sexo" value={formData.sexo} onChange={handleChange} style={inputStyle}>
                  <option value="" disabled>Selecione</option>
                  <option value="Masculino">Masculino</option>
                  <option value="Feminino">Feminino</option>
                </select>
              </div>
            </div>
          </div>
        )}

        {/* --- PASSO 2: MEDIDAS CORPORAIS --- */}
        {passoAtual === 2 && (
          <div>
             <h3 style={{ fontSize: '16px', color: '#059669', borderBottom: '1px solid #eee', paddingBottom: '5px' }}>Sua Estrutura Corporal</h3>
            <div style={{ display: 'flex', gap: '15px', marginBottom: '15px' }}>
              <div style={{ flex: 1 }}>
                <label style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}>Peso (kg) *</label>
                <input type="number" step="0.1" name="peso" value={formData.peso} onChange={handleChange} placeholder="Ex: 70.5" style={inputStyle} min="0" />
              </div>
              <div style={{ flex: 1 }}>
                <label style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}>Altura (cm) *</label>
                <input type="number" name="altura" value={formData.altura} onChange={handleChange} placeholder="Ex: 175" style={inputStyle} min="0" />
              </div>
            </div>

            <div style={{ marginBottom: '15px', backgroundColor: '#f8fafc', padding: '10px', borderLeft: '3px solid #f59e0b', borderRadius: '4px' }}>
              <label style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}>Percentual de Gordura (%)</label>
              <input type="number" step="0.1" name="percentualGordura" value={formData.percentualGordura} onChange={handleChange} placeholder="Ex: 15.5" style={inputStyle} min="0" max="100" />
              <p style={{ margin: '5px 0 0 0', fontSize: '11px', color: '#64748b' }}>Opcional. Preencha apenas se souber com precisão (ex: via bioimpedância). Se não souber, deixe em branco.</p>
            </div>
          </div>
        )}

        {/* --- PASSO 3: ESTILO DE VIDA --- */}
        {passoAtual === 3 && (
          <div>
            <h3 style={{ fontSize: '16px', color: '#059669', borderBottom: '1px solid #eee', paddingBottom: '5px' }}>Seu Estilo de Vida e Objetivo</h3>
            
            <div style={{ marginBottom: '15px' }}>
              <label style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}>Fator de Atividade *</label>
              <select name="fatorAtividade" value={formData.fatorAtividade} onChange={handleChange} style={inputStyle}>
                <option value="1.2">Sedentário (Trabalho de escritório, pouco exercício)</option>
                <option value="1.375">Levemente Ativo (Exercício leve 1-3 dias/semana)</option>
                <option value="1.55">Moderadamente Ativo (Exercício moderado 3-5 dias/semana)</option>
                <option value="1.725">Muito Ativo (Exercício pesado 6-7 dias/semana)</option>
                <option value="1.9">Extremamente Ativo (Atleta, trabalho físico pesado)</option>
              </select>
            </div>

            <div style={{ marginBottom: '15px' }}>
              <label style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}>Objetivo Físico *</label>
              <select name="objetivoFisico" value={formData.objetivoFisico} onChange={handleChange} style={inputStyle}>
                <option value="Manutencao">Manutenção do Peso Atual</option>
                <option value="Emagrecimento">Emagrecimento (Déficit Calórico)</option>
                <option value="GanhoDeMassa">Ganho de Massa (Superávit Calórico)</option>
              </select>
            </div>
          </div>
        )}

        {/* Tratamento de Erros Visual */}
        {erro && <p style={{ color: 'red', fontSize: '13px', margin: '5px 0', textAlign: 'center' }}>{erro}</p>}

        {/* Botões de Ação Dinâmicos */}
        <div style={buttonRowStyle}>
          {passoAtual > 1 && (
            <button type="button" onClick={voltarPasso} style={{ ...buttonStyle, backgroundColor: '#6c757d' }} disabled={carregando}>
              Voltar
            </button>
          )}
          
          {passoAtual < 3 ? (
            <button type="button" onClick={avancarPasso} style={{ ...buttonStyle, backgroundColor: '#007bff' }}>
              Avançar
            </button>
          ) : (
            <button type="button" onClick={handleSubmit} style={{ ...buttonStyle, backgroundColor: '#28a745' }} disabled={carregando}>
              {carregando ? 'Salvando...' : 'Concluir'}
            </button>
          )}
        </div>

      </form>
    </div>
  );
}